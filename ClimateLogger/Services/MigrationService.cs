using ClimateLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClimateLogger.Services;

internal sealed class MigrationService(IServiceProvider serviceProvider, ILogger<MigrationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            await context.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("Database migrations applied successfully");
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "Exception occured while applying database migrations");

            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}