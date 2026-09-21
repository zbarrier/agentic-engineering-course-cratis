namespace CratisApp.Specs.ShiftManagement.Scheduling.AssignEmployeeToShift.for_AssignEmployeeToShift;

using Cratis.Monads;
using CratisApp.ShiftManagement.Scheduling;
using AssignEmployeeToShiftCommand = CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift.AssignEmployeeToShift;
using CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift;

public class when_the_employee_is_already_assigned_to_the_shift : Specification
{
    AssignEmployeeToShiftCommand _command;
    ExistingShift _existingShift;
    ShiftAssignments _shiftAssignments;
    Result<EmployeeAssignedToShift, AssignEmployeeToShiftError> _result;

    void Establish()
    {
        var shiftId = ShiftId.New();
        var employeeId = EmployeeId.New();
        _existingShift = new(shiftId);
        _shiftAssignments = new(shiftId, [employeeId]);
        _command = new(shiftId, employeeId);
    }

    void Because() => _result = _command.Handle(_existingShift, _shiftAssignments);

    [Fact] void should_not_be_successful() => _result.IsSuccess.ShouldBeFalse();

    [Fact] void should_fail_with_employee_already_assigned()
    {
        _result.TryGetError(out var error);
        error.ShouldEqual(AssignEmployeeToShiftError.EmployeeAlreadyAssigned);
    }
}
