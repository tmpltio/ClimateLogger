using System.Configuration;
using System.Net;
using Microsoft.Extensions.Logging;

namespace ClimateLogger.Configuration;

internal static class Manager
{
    static Manager()
    {
        var settingsSection = (SettingsSection)ConfigurationManager.GetSection("settings");
        LoggerLevel = settingsSection.Logger.Level;
        DatabaseLocation = settingsSection.Database.Location;
        ControllerEndPoint = new IPEndPoint(settingsSection.Controller.Address, settingsSection.Controller.Port);
        RetryDelay = settingsSection.Retry.Delay;
    }

    public static LogLevel LoggerLevel { get; }

    public static string DatabaseLocation { get; }

    public static IPEndPoint ControllerEndPoint { get; }

    public static TimeSpan RetryDelay { get; }
}