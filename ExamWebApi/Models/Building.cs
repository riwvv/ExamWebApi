#nullable disable

namespace ExamWebApi.Models; 
public class Building {
    public string Adress { get; set; } = string.Empty;
    public int TotalFloors { get; set; }
    public ICollection<Elevator> Elevators { get; set; } = [];  
}
