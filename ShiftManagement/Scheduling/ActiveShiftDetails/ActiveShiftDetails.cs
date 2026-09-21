namespace CratisApp.ShiftManagement.Scheduling.ActiveShiftDetails;

using CratisApp.ShiftManagement.Scheduling;
using CratisApp.ShiftManagement.Scheduling.CreateShift;
using MongoDB.Driver;

[ReadModel]
[FromEvent<ShiftCreated>]
public record ActiveShiftDetails(
    ShiftId ShiftId,
    string Name,
    string Type,
    bool Recurring,
    string FromDay,
    string ToDay,
    DateTime StartDateTime,
    DateTime EndDateTime)
{
    public static async Task<ActiveShiftDetails?> GetActiveShiftDetails(ShiftId shiftId, IMongoCollection<ActiveShiftDetails> collection) =>
        await collection.Find(details => details.ShiftId == shiftId).FirstOrDefaultAsync();
}
