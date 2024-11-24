using System.ComponentModel;
using System.Configuration;

namespace ClimateLogger.Configuration;

internal sealed class RetryElement : ConfigurationElement
{
    [TypeConverter(typeof(TimeSpanConverter))]
    [ConfigurationProperty("delay", IsRequired = true)]
    public TimeSpan Delay =>
        (TimeSpan)this["delay"];
}