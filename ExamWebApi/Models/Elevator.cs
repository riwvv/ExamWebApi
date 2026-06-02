#nullable disable
namespace ExamWebApi.Models;

public class Elevator {
    public Guid SerialNumber { get; set; }
    public string ModelID { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }
    public int MinFloor { get; set; }
    public int MaxFloor { get; set; }
    public int CurrentFloor { get; set; }
    public string MoveStatus { get; set; } = "idle";
    public decimal MoveSpeed { get; set; }
    public string Status { get; set; } = "active";
    public Guid BuildingId { get; set; }
    public Building Building { get; set; }
    public ICollection<FloorCall> FloorCalls { get; set; } = [];
    public ICollection<TripLog> TripLogs { get; set; } = [];
}
