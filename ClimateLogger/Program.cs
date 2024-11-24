using ClimateLogger.Configuration;
using ClimateLogger.Data;
using ClimateLogger.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClimateLogger;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddDbContext<Context>(options => options.UseSqlite(Manager.DatabaseLocation));
                services.AddHostedService<MigrationService>();
                services.AddHostedService<ClimateLoggerService>();
            })
            .ConfigureLogging(builder => builder.SetMinimumLevel(Manager.LoggerLevel))
            .Build();

        try
        {
            await host.RunAsync();
        }
        catch
        { }
    }
}