#nullable disable
namespace ExamWebApi.Models;

public class TripLog {
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal DistanceTraveled { get; set; }
    public int TotalSeconds { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid ElevatorId { get; set; }
    public Elevator Elevator { get; set; }
}
