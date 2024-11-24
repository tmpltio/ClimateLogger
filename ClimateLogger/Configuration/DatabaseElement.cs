using System.Configuration;

namespace ClimateLogger.Configuration;

internal sealed class DatabaseElement : ConfigurationElement
{
    [ConfigurationProperty("location", IsRequired = true)]
    public string Location =>
        (string)this["location"];
}