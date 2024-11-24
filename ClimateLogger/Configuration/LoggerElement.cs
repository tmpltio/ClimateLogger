using System.ComponentModel;
using System.Configuration;
using Microsoft.Extensions.Logging;

namespace ClimateLogger.Configuration;

internal sealed class LoggerElement : ConfigurationElement
{
    [TypeConverter(typeof(LogLevelConverter))]
    [ConfigurationProperty("level", IsRequired = true)]
    public LogLevel Level =>
        (LogLevel)this["level"];
}