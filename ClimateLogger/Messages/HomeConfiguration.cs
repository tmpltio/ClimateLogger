using System.Text.Json.Serialization;

namespace ClimateLogger.Messages;

public record class HomeConfiguration
{
    [JsonPropertyName("rooms")]
    public required IReadOnlyCollection<RoomConfiguration> Rooms { init; get; }
}