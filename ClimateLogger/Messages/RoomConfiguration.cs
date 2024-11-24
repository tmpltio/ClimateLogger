using System.Text.Json.Serialization;

namespace ClimateLogger.Messages;

public record class RoomConfiguration
{
    [JsonPropertyName("name")]
    public required string Name { init; get; }

    [JsonPropertyName("port")]
    public required int Port { init; get; }
}