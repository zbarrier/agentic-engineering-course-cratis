namespace CratisApp.Specs.ShiftManagement.Scheduling.CreateShift.for_CreateShiftValidator;

using FluentValidation.Results;
using CreateShiftCommand = CratisApp.ShiftManagement.Scheduling.CreateShift.CreateShift;
using CratisApp.ShiftManagement.Scheduling.CreateShift;

public class when_from_day_is_after_to_day : Specification
{
    CreateShiftCommand _command;
    CreateShiftValidator _validator;
    ValidationResult _result;

    void Establish()
    {
        _command = new(
            "Evening Service",
            "Service",
            false,
            "Wednesday",
            "Monday",
            new DateTime(2026, 9, 21, 18, 0, 0),
            new DateTime(2026, 9, 21, 22, 0, 0));
        _validator = new();
    }

    void Because() => _result = _validator.Validate(_command);

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
}
