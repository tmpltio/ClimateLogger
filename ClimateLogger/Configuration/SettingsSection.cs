using System.Configuration;

namespace ClimateLogger.Configuration;

internal sealed class SettingsSection : ConfigurationSection
{
    [ConfigurationProperty("database", IsRequired = true)]
    public DatabaseElement Database =>
        (DatabaseElement)this["database"];

    [ConfigurationProperty("controller", IsRequired = true)]
    public ControllerElement Controller =>
        (ControllerElement)this["controller"];

    [ConfigurationProperty("retry", IsRequired = true)]
    public RetryElement Retry =>
        (RetryElement)this["retry"];

    [ConfigurationProperty("logger", IsRequired = true)]
    public LoggerElement Logger =>
        (LoggerElement)this["logger"];
}