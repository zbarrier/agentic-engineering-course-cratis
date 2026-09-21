namespace CratisApp.ShiftManagement.Scheduling.ShiftSummary;

using CratisApp.ShiftManagement.Scheduling;
using CratisApp.ShiftManagement.Scheduling.CreateShift;
using MongoDB.Driver;

[ReadModel]
[FromEvent<ShiftCreated>]
public record ShiftSummary(
    ShiftId ShiftId,
    string Name,
    string Type,
    DateTime StartDateTime,
    DateTime EndDateTime)
{
    public static async Task<ShiftSummary?> GetShiftSummary(ShiftId shiftId, IMongoCollection<ShiftSummary> collection) =>
        await collection.Find(summary => summary.ShiftId == shiftId).FirstOrDefaultAsync();
}
