namespace CratisApp.Specs.ShiftManagement.Scheduling.ShiftSummary.for_ShiftSummary;

using CratisApp.ShiftManagement.Scheduling;
using MongoDB.Driver;
using NSubstitute;
using ShiftSummaryModel = CratisApp.ShiftManagement.Scheduling.ShiftSummary.ShiftSummary;

public class when_getting_the_shift_summary_by_id : Specification
{
    IMongoCollection<ShiftSummaryModel> _collection;
    ShiftId _shiftId;
    ShiftSummaryModel _existing;
    ShiftSummaryModel? _result;

    void Establish()
    {
        _shiftId = ShiftId.New();
        _existing = new(
            _shiftId,
            "Morning Kitchen",
            "Kitchen",
            new DateTime(2026, 9, 21, 8, 0, 0),
            new DateTime(2026, 9, 21, 14, 0, 0));

        var cursor = Substitute.For<IAsyncCursor<ShiftSummaryModel>>();
        cursor.Current.Returns([_existing]);
        cursor.MoveNextAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(true), Task.FromResult(false));

        _collection = Substitute.For<IMongoCollection<ShiftSummaryModel>>();
        _collection.FindAsync(
            Arg.Any<FilterDefinition<ShiftSummaryModel>>(),
            Arg.Any<FindOptions<ShiftSummaryModel, ShiftSummaryModel>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cursor));
    }

    async Task Because() => _result = await ShiftSummaryModel.GetShiftSummary(_shiftId, _collection);

    [Fact] void should_return_the_matching_shift_summary() => _result.ShouldEqual(_existing);
}
