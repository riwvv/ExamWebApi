namespace ExamWebApi.DTOs;

public class BuildingResponseDto {
    public Guid Id { get; set; }
    public string Adress { get; set; } = string.Empty;
    public int TotalFloors { get; set; }
    public List<ElevatorResponseDto> Elevators { get; set; } = new();
}
