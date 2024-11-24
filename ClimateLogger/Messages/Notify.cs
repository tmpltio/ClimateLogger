using System.Text.Json.Serialization;

namespace ClimateLogger.Messages;

public record class Notify
{
    [JsonPropertyName("type")]
    public required string Type { init; get; }

    [JsonPropertyName("success")]
    public required bool Success { init; get; }

    [JsonPropertyName("status")]
    public Status? Status { init; get; }
}