using System.Text.Json.Serialization;

namespace ClimateLogger.Messages;

public record class Status
{
    [JsonPropertyName("current_temperature")]
    public required decimal CurrentTemperature { init; get; }

    [JsonPropertyName("target_temperature")]
    public decimal? TargetTemperature { init; get; }

    [JsonPropertyName("current_humidity")]
    public byte? CurrentHumidity { init; get; }

    [JsonPropertyName("current_state")]
    public string? CurrentState { init; get; }

    [JsonPropertyName("target_state")]
    public string? TargetState { init; get; }
}