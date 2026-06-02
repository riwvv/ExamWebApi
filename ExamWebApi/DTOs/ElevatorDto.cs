#nullable disable
namespace ExamWebApi.DTOs;

public class ElevatorDto {
    public string ModelID { get; set; } = string.Empty;
    public int MinFloor { get; set; }
    public int MaxFloor { get; set; }
    public int CurrentFloor { get; set; }
    public string MoveStatus { get; set; } = "idle";
    public decimal MoveSpeed { get; set; }
    public decimal Mileage { get; set; }
    public string Status { get; set; } = "active";
    public string BuildingAdress { get; set; } = string.Empty;
}
