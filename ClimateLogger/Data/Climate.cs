namespace ClimateLogger.Data;

public class Climate
{
    public int Id { get; set; }

    public required DateTime Timestamp { get; set; }

    public int RoomId { get; set; }

    public required Room Room { get; set; }

    public required decimal CurrentTemperature { get; set; }

    public decimal? TargetTemperature { get; set; }

    public byte? CurrentHumidity { get; set; }

    public int? CurrentStateId { get; set; }

    public State? CurrentState { get; set; }

    public int? TargetStateId { get; set; }

    public State? TargetState { get; set; }
}