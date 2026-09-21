namespace CratisApp.Specs.ShiftManagement.Scheduling.ActiveShiftDetails.for_ActiveShiftDetails;

using CratisApp.ShiftManagement.Scheduling;
using MongoDB.Driver;
using NSubstitute;
using ActiveShiftDetailsModel = CratisApp.ShiftManagement.Scheduling.ActiveShiftDetails.ActiveShiftDetails;

public class when_getting_the_active_shift_details_by_id : Specification
{
    IMongoCollection<ActiveShiftDetailsModel> _collection;
    ShiftId _shiftId;
    ActiveShiftDetailsModel _existing;
    ActiveShiftDetailsModel? _result;

    void Establish()
    {
        _shiftId = ShiftId.New();
        _existing = new(
            _shiftId,
            "Morning Kitchen",
            "Kitchen",
            true,
            "Monday",
            "Friday",
            new DateTime(2026, 9, 21, 8, 0, 0),
            new DateTime(2026, 9, 21, 14, 0, 0));

        var cursor = Substitute.For<IAsyncCursor<ActiveShiftDetailsModel>>();
        cursor.Current.Returns([_existing]);
        cursor.MoveNextAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(true), Task.FromResult(false));

        _collection = Substitute.For<IMongoCollection<ActiveShiftDetailsModel>>();
        _collection.FindAsync(
            Arg.Any<FilterDefinition<ActiveShiftDetailsModel>>(),
            Arg.Any<FindOptions<ActiveShiftDetailsModel, ActiveShiftDetailsModel>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cursor));
    }

    async Task Because() => _result = await ActiveShiftDetailsModel.GetActiveShiftDetails(_shiftId, _collection);

    [Fact] void should_return_the_matching_active_shift_details() => _result.ShouldEqual(_existing);
}
