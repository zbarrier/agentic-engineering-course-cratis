namespace CratisApp.Specs.ShiftManagement.Scheduling.ShiftAssignments.for_ShiftAssignments;

using CratisApp.ShiftManagement.Scheduling;
using MongoDB.Driver;
using NSubstitute;
using ShiftAssignmentsModel = CratisApp.ShiftManagement.Scheduling.ShiftAssignments.ShiftAssignments;

public class when_getting_assignments_for_a_shift_with_no_employees_assigned_yet : Specification
{
    IMongoCollection<ShiftAssignmentsModel> _collection;
    ShiftId _shiftId;
    ShiftAssignmentsModel _result;

    void Establish()
    {
        _shiftId = ShiftId.New();

        var cursor = Substitute.For<IAsyncCursor<ShiftAssignmentsModel>>();
        cursor.Current.Returns([]);
        cursor.MoveNextAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(false));

        _collection = Substitute.For<IMongoCollection<ShiftAssignmentsModel>>();
        _collection.FindAsync(
            Arg.Any<FilterDefinition<ShiftAssignmentsModel>>(),
            Arg.Any<FindOptions<ShiftAssignmentsModel, ShiftAssignmentsModel>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(cursor));
    }

    async Task Because() => _result = await ShiftAssignmentsModel.GetShiftAssignments(_shiftId, _collection);

    [Fact] void should_return_an_empty_list_of_employee_ids() => _result.EmployeeIds.ShouldBeEmpty();
}
