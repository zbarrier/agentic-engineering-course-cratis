namespace CratisApp.ShiftManagement.Scheduling.ShiftAssignments;

using CratisApp.ShiftManagement.Scheduling;
using CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift;
using MongoDB.Driver;

[ReadModel]
public record ShiftAssignments(ShiftId ShiftId, IEnumerable<EmployeeId> EmployeeIds)
{
    public static async Task<ShiftAssignments> GetShiftAssignments(ShiftId shiftId, IMongoCollection<ShiftAssignments> collection)
    {
        var existing = await collection.Find(assignments => assignments.ShiftId == shiftId).FirstOrDefaultAsync();
        return existing ?? new ShiftAssignments(shiftId, []);
    }
}

public class ShiftAssignmentsReducer : IReducerFor<ShiftAssignments>
{
    public ShiftAssignments EmployeeAssignedToShift(EmployeeAssignedToShift @event, ShiftAssignments? current, EventContext context) =>
        (current ?? new(@event.ShiftId, [])) with
        {
            EmployeeIds = [.. current?.EmployeeIds ?? [], @event.EmployeeId]
        };
}
