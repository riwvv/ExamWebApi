using ExamWebApi.Data;
using ExamWebApi.DTOs;
using ExamWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamWebApi.Services;

public class CreateElevatorService(AppDbContext _context, ILogger<CreateElevatorService> _logger) {
    public async Task<Elevator?> CreateNewElevatorAsync(ElevatorDto elevator) {
        if (elevator == null) {
            _logger.LogWarning("Получен пустой объект при создании лифта");
            return null;
        }

        var building = await _context.Buildings.FirstOrDefaultAsync(x => x.Adress == elevator.BuildingAdress);

        if (building == null) {
            _logger.LogWarning("Получен несуществующий адрес здания для нового лифта");
            return null;
        }

        if (elevator.Status != "active" && elevator.Status != "maintenance" && elevator.Status != "alarm") {
            _logger.LogWarning("Получен недопустимый статус для лифта");
            return null;
        }

        try {
            var newElevator = new Elevator {
                SerialNumber = Guid.NewGuid(),
                ModelID = elevator.ModelID,
                ProductionDate = DateTime.Now,
                MinFloor = elevator.MinFloor,
                MaxFloor = elevator.MaxFloor,
                MoveSpeed = elevator.MoveSpeed,
                Status = elevator.Status,
                Building = building
            };

            await _context.Elevators.AddAsync(newElevator);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Создание нового лифта прошло успешно");

            return newElevator;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Ошибка при создании лифта");
            return null;
        }
    }
}
