namespace ExamWebApi.DTOs;

public class BuildingResponseDto {
    public string Adress { get; set; } = string.Empty;
    public int TotalFloors { get; set; }
    public List<ElevatorResponseDto> Elevators { get; set; } = new();
}
