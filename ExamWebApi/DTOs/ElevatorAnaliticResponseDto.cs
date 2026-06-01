#nullable disable
using ExamWebApi.Models;

namespace ExamWebApi.DTOs;

public class ElevatorAnaliticResponseDto {
    public Guid SerialNumber { get; set; }
    public string ModelID { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }
    public int MinFloor { get; set; }
    public int MaxFloor { get; set; }
    public decimal MoveSpeed { get; set; }
    public string Status { get; set; } = "active";
    public BuildingDto Building { get; set; }
    public List<FloorCallDto> FloorCalls { get; set; } = [];
    public List<TripLogDto> TripLogs { get; set; } = [];
}
