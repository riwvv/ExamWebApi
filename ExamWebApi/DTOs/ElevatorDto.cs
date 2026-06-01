#nullable disable
namespace ExamWebApi.DTOs;

public class ElevatorDto {
    public string ModelID { get; set; } = string.Empty;
    public int MinFloor { get; set; }
    public int MaxFloor { get; set; }
    public decimal MoveSpeed { get; set; }
    public string Status { get; set; } = "active";
    public string BuildingAdress { get; set; } = string.Empty;
}
