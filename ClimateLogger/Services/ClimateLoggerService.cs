using System.Net.Sockets;
using System.Text.Json;
using ClimateLogger.Configuration;
using ClimateLogger.Data;
using ClimateLogger.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClimateLogger.Services;

internal sealed class ClimateLoggerService(IServiceProvider serviceProvider, ILogger<ClimateLoggerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting climate logger service");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var roomsConfiguration = await GetRoomsConfigurationAsync(stoppingToken);

                logger.LogInformation("Received configuration for {count} rooms", roomsConfiguration.Count);

                var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                var roomsTasks = roomsConfiguration
                    .Select(roomConfiguration => ListenFromRoomAsync(roomConfiguration.Name, roomConfiguration.Port, cancellationTokenSource.Token));
                var finishedTask = await Task.WhenAny(roomsTasks);
                await cancellationTokenSource.CancelAsync();
                await finishedTask;
            }
            catch (OperationCanceledException)
            { }
            catch (Exception exception)
            {
                logger.LogError(exception, "Unexpected error");

                await Task.Delay(Manager.RetryDelay, stoppingToken);
            }
        }

        logger.LogInformation("Climate logger service stopped");
    }

    private static async Task<IReadOnlyCollection<RoomConfiguration>> GetRoomsConfigurationAsync(CancellationToken cancellationToken)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(Manager.ControllerEndPoint, cancellationToken);

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);

        var json = await reader.ReadToEndAsync(cancellationToken);
        var configuration = JsonSerializer.Deserialize<HomeConfiguration>(json);

        return configuration
            ?.Rooms
            ?? throw new InvalidDataException("Invalid configuration received");
    }

    private async Task ListenFromRoomAsync(string name, int port, CancellationToken cancellationToken)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(Manager.ControllerEndPoint.Address, port, cancellationToken);

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        var buffer = new char[1024];

        while (!cancellationToken.IsCancellationRequested)
        {
            var read = await reader.ReadAsync(buffer, cancellationToken);
            var notify = JsonSerializer.Deserialize<Notify>(buffer.AsSpan()[..read]);

            if (notify is null || notify.Status is null || !notify.Success)
            {
                continue;
            }

            logger.LogInformation("Received {type} message from {name}: {status}", notify.Type, name, notify.Status);

            using var scope = serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<Context>();
            await context.WriteClimateAsync(name, notify.Status, cancellationToken);
        }
    }
}