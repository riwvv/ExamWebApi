using ExamWebApi.Data;
using ExamWebApi.Models;

namespace ExamWebApi.Services;

public class ActionElevatorService(AppDbContext _context, ILogger<SearchElevatorService> _logger) {
    public async Task<string> StartElevatorAsync(Guid elevatorId, int target) {
		try {
			var elevator = await _context.Elevators.FindAsync(elevatorId);
            if (elevator == null) {
				_logger.LogError($"Лифт с id '{elevatorId}' не найден");
				return string.Empty;
			}

			if (elevator.MinFloor > target || elevator.MaxFloor < target) {
				_logger.LogError($"Этаж {target} недоступен для лифта '{elevatorId}'");
				return string.Empty;
			}

			if (elevator.CurrentFloor == target) {
				_logger.LogError("Лифт уже на этаже назначения");
				return string.Empty;
			}

			var log1 = new TripLog {
				Id = Guid.NewGuid(),
				Description = $"Пользователь зашёл и нажал {target} этаж. Поездка началась {DateTime.Now.ToString("f")}.",
				DistanceTraveled = Math.Abs(elevator.CurrentFloor - target) * 3,
				TotalSeconds = Convert.ToInt32(Math.Abs(elevator.CurrentFloor - target) * 3 / elevator.MoveSpeed),
				Timestamp = DateTime.Now,
				ElevatorId = elevatorId,
				Elevator = elevator
			};

			var status = target > elevator.CurrentFloor ? "up" : "down";

			(await _context.Elevators.FindAsync(elevatorId))!.Status = status;
			await _context.TripLogs.AddAsync(log1);
			await _context.SaveChangesAsync();

			await Task.Delay(log1.TotalSeconds * 1000);

			var log2 = new TripLog {
				Id = Guid.NewGuid(),
				Description = $"Поездка завершена {DateTime.Now.ToString("f")}.",
				DistanceTraveled = log1.DistanceTraveled,
				TotalSeconds = log1.TotalSeconds,
				Timestamp = DateTime.Now,
				ElevatorId = elevatorId,
				Elevator = elevator
			};

			await _context.TripLogs.AddAsync(log2);
			(await _context.Elevators.FindAsync(elevatorId))!.Status = "idle";
			(await _context.Elevators.FindAsync(elevatorId))!.CurrentFloor = target;
			(await _context.Elevators.FindAsync(elevatorId))!.Mileage += log1.DistanceTraveled;
			await _context.SaveChangesAsync();

			return $"Вы прибыли на {target} этаж";
		}
		catch (Exception ex) {
			_logger.LogError(ex, "Ошибка при запуске лифта");
			return string.Empty;
		}
    }
}
