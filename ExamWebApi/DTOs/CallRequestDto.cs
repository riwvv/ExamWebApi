namespace ExamWebApi.DTOs;

public class CallRequestDto {
    public Guid BuildingId { get; set; }
    public int FloorNumber { get; set; }
    public string Direction { get; set; } = string.Empty;
}
