namespace ExamWebApi.DTOs;

public class CallResponseDto {
    public Guid ElevatorId { get; set; }
    public TimeOnly ArrivalTime { get; set; }
}
