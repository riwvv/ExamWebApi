using ExamWebApi.DTOs;

namespace ExamWebApi.Wrapper;

public static class MyWrapper {
    public static bool TryFind(IEnumerable<ElevatorResponseDto>? values, int desired, out ElevatorResponseDto? result) {
        if (values == null) {
            result = default;
            return false;
        }

        var res = values.FirstOrDefault(x => x.CurrentFloor == desired);
        if (res == null) {
            result = default;
            return false;
        }
        
        result = res;
        return true;
    }

    public static bool TryFindAbove(IEnumerable<ElevatorResponseDto>? values, int desired, out IEnumerable<ElevatorResponseDto>? result) {
        if (values == null) {
            result = default;
            return false;
        }

        var res = values.Where(x => x.MinFloor >= desired && x.MaxFloor <= desired && x.CurrentFloor > desired && (x.MoveStatus == "down" || x.MoveStatus == "idle"));
        if (res == null) {
            result = default;
            return false;
        }

        result = res;
        return true;
    }

    public static bool TryFindBelow(IEnumerable<ElevatorResponseDto>? values, int desired, out IEnumerable<ElevatorResponseDto>? result) {
        if (values == null) {
            result = default;
            return false;
        }

        var res = values.Where(x => x.MinFloor >= desired && x.MaxFloor <= desired && x.CurrentFloor < desired && (x.MoveStatus == "up" || x.MoveStatus == "idle"));
        if (res == null) {
            result = default;
            return false;
        }

        result = res;
        return true;
    }
}
