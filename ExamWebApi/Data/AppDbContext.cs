using ExamWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamWebApi.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Elevator> Elevators { get; set; }
    public DbSet<FloorCall> FloorCalls { get; set; }
    public DbSet<TripLog> TripLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Building>(entity => {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Adress).IsRequired().HasMaxLength(100);
            entity.Property(x => x.TotalFloors).IsRequired();

            entity.HasMany(x => x.Elevators).WithOne(c => c.Building).HasForeignKey(c => c.BuildingId).OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.TotalFloors);
        });

        modelBuilder.Entity<Elevator>(entity => {
            entity.HasKey(x => x.SerialNumber);

            entity.Property(x => x.ModelID).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ProductionDate).IsRequired();
            entity.Property(x => x.MinFloor).IsRequired();
            entity.Property(x => x.MaxFloor).IsRequired();
            entity.Property(x => x.CurrentFloor).IsRequired();
            entity.Property(x => x.MoveStatus).HasMaxLength(20).IsRequired();
            entity.Property(x => x.MoveSpeed).HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.Mileage).HasPrecision(18, 2);
            entity.Property(x => x.Status).HasMaxLength(11).IsRequired();

            entity.HasMany(x => x.FloorCalls).WithOne(c => c.Elevator).HasForeignKey(c => c.ElevatorId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.TripLogs).WithOne(c => c.Elevator).HasForeignKey(c => c.ElevatorId).OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.ModelID);
        });

        modelBuilder.Entity<FloorCall>(entity => {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CurrentFloor).IsRequired();
            entity.Property(x => x.Course).HasMaxLength(4);
            entity.Property(x => x.Timestamp).IsRequired();

            entity.HasIndex(x => x.Timestamp);
        });

        modelBuilder.Entity<TripLog>(entity => {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.TripId).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(200).IsRequired();
            entity.Property(x => x.DistanceTraveled).HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.TotalSeconds).IsRequired();
            entity.Property(x => x.Timestamp).IsRequired();

            entity.HasIndex(x => x.Description);
            entity.HasIndex(x => x.Timestamp);
        });

        base.OnModelCreating(modelBuilder);
    }
}
