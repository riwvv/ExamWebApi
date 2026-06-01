using ExamWebApi.Data;
using ExamWebApi.DTOs;
using ExamWebApi.Models;

namespace ExamWebApi.Services;

public class CreateBuildingService(AppDbContext _context, ILogger<CreateBuildingService> _logger) {
    public async Task<Building?> CreateNewBuildingAsync(BuildingDto building) {
        if (building == null) {
            _logger.LogWarning("Получен пустой объект при создании нового здания");
            return null;
        }

        try {
            var newBuilding = new Building {
                Adress = building.Adress,
                TotalFloors = building.TotalFloors
            };

            await _context.Buildings.AddAsync(newBuilding);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Создание нового здания прошло успешно");
            
            return newBuilding;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Ошибка при создании здания");
            return null;
        }
    }
}
