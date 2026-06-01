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
            entity.HasKey(x => x.Adress);

            entity.Property(x => x.Adress).HasMaxLength(150);
            entity.Property(x => x.TotalFloors).IsRequired();

            entity.HasIndex(x => x.TotalFloors);
        });

        modelBuilder.Entity<Elevator>(entity => {
            entity.HasKey(x => x.SerialNumber);

            entity.Property(x => x.ModelID).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ProductionDate).IsRequired();
            entity.Property(x => x.MinFloor).IsRequired();
            entity.Property(x => x.MaxFloor).IsRequired();
            entity.Property(x => x.MoveSpeed).HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(11).IsRequired();

            entity.HasOne(x => x.Building).WithMany(x => x.Elevators).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.ModelID);
        });

        modelBuilder.Entity<FloorCall>(entity => {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CurrentFloor).IsRequired();
            entity.Property(x => x.Course).HasMaxLength(4);
            entity.Property(x => x.Timestamp).IsRequired();

            entity.HasOne(x => x.Elevator).WithMany(x => x.FloorCalls).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.Timestamp);
        });

        modelBuilder.Entity<TripLog>(entity => {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description).HasMaxLength(200).IsRequired();
            entity.Property(x => x.DistanceTraveled).HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.TotalSeconds).IsRequired();
            entity.Property(x => x.Timestamp).IsRequired();

            entity.HasOne(x => x.Elevator).WithMany(x => x.TripLogs).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.Description);
            entity.HasIndex(x => x.Timestamp);
        });

        base.OnModelCreating(modelBuilder);
    }
}
