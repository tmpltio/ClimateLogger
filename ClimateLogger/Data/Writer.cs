using ClimateLogger.Messages;
using Microsoft.EntityFrameworkCore;

namespace ClimateLogger.Data;

internal static class Writer
{
    public static async Task WriteClimateAsync(this Context context, string name, Status status, CancellationToken cancellationToken)
    {
        var currentState = await context.GetStateAsync(status.CurrentState, cancellationToken);
        var targetState = await context.GetStateAsync(status.TargetState, cancellationToken);
        var room = await context.GetRoomAsync(name, cancellationToken);
        var climate = new Climate
        {
            Timestamp = DateTime.Now,
            Room = room,
            CurrentTemperature = status.CurrentTemperature,
            TargetTemperature = status.TargetTemperature,
            CurrentHumidity = status.CurrentHumidity,
            CurrentState = currentState,
            TargetState = targetState
        };
        context.Climates.Add(climate);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<Room> GetRoomAsync(this Context context, string name, CancellationToken cancellationToken = default)
    {
        var room = await context
            .Rooms
            .FirstOrDefaultAsync(room => room.Name == name, cancellationToken);

        return room
            ?? new Room { Name = name };
    }

    private static async Task<State?> GetStateAsync(this Context context, string? name, CancellationToken cancellationToken)
    {
        if (name is null)
        {
            return null;
        }

        var state = await context
            .States
            .FirstOrDefaultAsync(state => state.Name == name, cancellationToken);

        return state
            ?? new State { Name = name };
    }
}