namespace CratisApp.Specs.ShiftManagement.Scheduling.ShiftAssignments.for_ShiftAssignments;

using CratisApp.ShiftManagement.Scheduling;
using MongoDB.Driver;
using NSubstitute;
using ShiftAssignmentsModel = CratisApp.ShiftManagement.Scheduling.ShiftAssignments.ShiftAssignments;

public class when_getting_assignments_for_a_shift_with_employees_assigned : Specification
{
    IMongoCollection<ShiftAssignmentsModel> _collection;
    ShiftId _shiftId;
    ShiftAssignmentsModel _existing;
    ShiftAssignmentsModel _result;

    void Establish()
    {
        _shiftId = ShiftId.New();
        _existing = new(_shiftId, [EmployeeId.New(), EmployeeId.New()]);

        var cursor = Substitute.For<IAsyncCursor<ShiftAssignmentsModel>>();
        cursor.Current.Returns([_existing]);
        cursor.MoveNextAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(true), Task.FromResult(false));

        _collection = Substitute.For<IMongoCollection<ShiftAssignmentsModel>>();
        _collection.FindAsync(
            Arg.Any<FilterDefinition<ShiftAssignmentsModel>>(),
            Arg.Any<FindOptions<ShiftAssignmentsModel, ShiftAssignmentsModel>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cursor));
    }

    async Task Because() => _result = await ShiftAssignmentsModel.GetShiftAssignments(_shiftId, _collection);

    [Fact] void should_return_the_matching_shift_assignments() => _result.ShouldEqual(_existing);
}
