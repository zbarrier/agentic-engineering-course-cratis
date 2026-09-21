namespace CratisApp.Specs.ShiftManagement.Scheduling.AssignEmployeeToShift.for_AssignEmployeeToShift;

using Cratis.Monads;
using CratisApp.ShiftManagement.Scheduling;
using AssignEmployeeToShiftCommand = CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift.AssignEmployeeToShift;
using CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift;

public class when_the_shift_exists_and_the_employee_is_not_assigned : Specification
{
    AssignEmployeeToShiftCommand _command;
    ShiftId _shiftId;
    EmployeeId _employeeId;
    ExistingShift _existingShift;
    Result<EmployeeAssignedToShift, AssignEmployeeToShiftError> _result;

    void Establish()
    {
        _shiftId = ShiftId.New();
        _employeeId = EmployeeId.New();
        _existingShift = new(_shiftId);
        _command = new(_shiftId, _employeeId);
    }

    void Because() => _result = _command.Handle(_existingShift, null);

    [Fact] void should_be_successful() => _result.IsSuccess.ShouldBeTrue();

    [Fact] void should_append_employee_assigned_to_shift_event()
    {
        _result.TryGetResult(out var @event);
        @event.ShiftId.ShouldEqual(_shiftId);
        @event.EmployeeId.ShouldEqual(_employeeId);
    }
}
