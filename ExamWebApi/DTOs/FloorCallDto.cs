#nullable disable
namespace ExamWebApi.DTOs;

public class FloorCallDto {
    public Guid Id { get; set; }
    public int CurrentFloor { get; set; }
    public string Course { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
