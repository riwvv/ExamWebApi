#nullable disable
namespace ExamWebApi.DTOs;

public class TripLogDto {
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal DistanceTraveled { get; set; }
    public int TotalSeconds { get; set; }
    public DateTime Timestamp { get; set; }
}
