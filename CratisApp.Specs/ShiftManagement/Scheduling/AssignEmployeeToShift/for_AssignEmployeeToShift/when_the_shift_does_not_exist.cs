namespace CratisApp.Specs.ShiftManagement.Scheduling.AssignEmployeeToShift.for_AssignEmployeeToShift;

using Cratis.Monads;
using CratisApp.ShiftManagement.Scheduling;
using AssignEmployeeToShiftCommand = CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift.AssignEmployeeToShift;
using CratisApp.ShiftManagement.Scheduling.AssignEmployeeToShift;

public class when_the_shift_does_not_exist : Specification
{
    AssignEmployeeToShiftCommand _command;
    Result<EmployeeAssignedToShift, AssignEmployeeToShiftError> _result;

    void Establish() => _command = new(ShiftId.New(), EmployeeId.New());

    void Because() => _result = _command.Handle(null, null);

    [Fact] void should_not_be_successful() => _result.IsSuccess.ShouldBeFalse();

    [Fact] void should_fail_with_shift_not_found()
    {
        _result.TryGetError(out var error);
        error.ShouldEqual(AssignEmployeeToShiftError.ShiftNotFound);
    }
}
