namespace ExamWebApi.DTOs;

public class ElevatorResponseDto {
    public Guid SerialNumber { get; set; }
    public string ModelID { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }
    public int MinFloor { get; set; }
    public int MaxFloor { get; set; }
    public decimal MoveSpeed { get; set; }
    public string Status { get; set; } = string.Empty;
}
