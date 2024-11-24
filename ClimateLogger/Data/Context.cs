using Microsoft.EntityFrameworkCore;

namespace ClimateLogger.Data;

public class Context(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Climate> Climates { get; set; } = null!;

    public DbSet<Room> Rooms { get; set; } = null!;

    public DbSet<State> States { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Climate>()
            .HasKey(data => data.Id);
        modelBuilder
            .Entity<Climate>()
            .Property(data => data.Timestamp)
            .IsRequired();
        modelBuilder
            .Entity<Climate>()
            .HasOne(data => data.Room)
            .WithMany(room => room.Climates)
            .HasForeignKey(data => data.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Climate>()
            .HasOne(data => data.CurrentState)
            .WithMany()
            .HasForeignKey(data => data.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder
            .Entity<Climate>()
            .HasOne(data => data.TargetState)
            .WithMany()
            .HasForeignKey(data => data.TargetStateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Room>()
            .HasKey(room => room.Id);
        modelBuilder
            .Entity<Room>()
            .Property(room => room.Name)
            .IsRequired();
        modelBuilder
            .Entity<Room>()
            .HasIndex(room => room.Name)
            .IsUnique();

        modelBuilder
            .Entity<State>()
            .HasKey(state => state.Id);
        modelBuilder
            .Entity<State>()
            .Property(state => state.Name)
            .IsRequired();
        modelBuilder
            .Entity<State>()
            .HasIndex(state => state.Name)
            .IsUnique();
    }
}