namespace CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift;

using Cratis.Chronicle.Keys;
using Cratis.Monads;
using CratisApp.ShiftManagement.Scheduling;
using CratisApp.ShiftManagement.Scheduling.CreateShift;

[Command]
public record AssignEmployeeToShift([Key] ShiftId ShiftId, EmployeeId EmployeeId)
{
    public Result<EmployeeAssignedToShift, AssignEmployeeToShiftError> Handle(
        ExistingShift? existingShift,
        ShiftAssignments? shiftAssignments)
    {
        if (existingShift is null)
            return AssignEmployeeToShiftError.ShiftNotFound;

        if (shiftAssignments is not null && shiftAssignments.EmployeeIds.Contains(EmployeeId))
            return AssignEmployeeToShiftError.EmployeeAlreadyAssigned;

        return new EmployeeAssignedToShift(ShiftId, EmployeeId);
    }
}

public enum AssignEmployeeToShiftError
{
    ShiftNotFound,
    EmployeeAlreadyAssigned
}

[EventType]
public record EmployeeAssignedToShift(ShiftId ShiftId, EmployeeId EmployeeId);

[ReadModel]
[FromEvent<ShiftCreated>]
public record ExistingShift(ShiftId ShiftId);

[ReadModel]
public record ShiftAssignments(ShiftId ShiftId, IEnumerable<EmployeeId> EmployeeIds);

public class ShiftAssignmentsReducer : IReducerFor<ShiftAssignments>
{
    public ShiftAssignments EmployeeAssignedToShift(EmployeeAssignedToShift @event, ShiftAssignments? current, EventContext context) =>
        (current ?? new(@event.ShiftId, [])) with
        {
            EmployeeIds = [.. current?.EmployeeIds ?? [], @event.EmployeeId]
        };
}
