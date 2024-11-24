namespace ClimateLogger.Data;

public class Room
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Climate> Climates { get; set; } = [];
}