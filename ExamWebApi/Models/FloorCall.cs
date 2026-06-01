#nullable disable
namespace ExamWebApi.Models;

public class FloorCall {
    public Guid Id { get; set; }
    public int CurrentFloor { get; set; }
    public string Course { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Elevator Elevator { get; set; } = null;
}
