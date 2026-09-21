namespace CratisApp.Specs.ShiftManagement.Scheduling.CreateShift.for_CreateShift;

using CratisApp.ShiftManagement.Scheduling;
using CreateShiftCommand = CratisApp.ShiftManagement.Scheduling.CreateShift.CreateShift;
using CratisApp.ShiftManagement.Scheduling.CreateShift;

public class when_creating_a_valid_shift : Specification
{
    CreateShiftCommand _command;
    ShiftId _id;
    ShiftCreated _event;

    void Establish() => _command = new(
        "Morning Kitchen",
        "Kitchen",
        false,
        "Monday",
        "Monday",
        new DateTime(2026, 9, 21, 8, 0, 0),
        new DateTime(2026, 9, 21, 14, 0, 0));

    void Because() => (_id, _event) = _command.Handle();

    [Fact] void should_carry_the_generated_shift_id() => _event.ShiftId.ShouldEqual(_id);
    [Fact] void should_carry_the_name() => _event.Name.ShouldEqual("Morning Kitchen");
    [Fact] void should_carry_the_type() => _event.Type.ShouldEqual("Kitchen");
    [Fact] void should_carry_recurring() => _event.Recurring.ShouldEqual(false);
    [Fact] void should_carry_from_day() => _event.FromDay.ShouldEqual("Monday");
    [Fact] void should_carry_to_day() => _event.ToDay.ShouldEqual("Monday");
    [Fact] void should_carry_start_date_time() => _event.StartDateTime.ShouldEqual(new DateTime(2026, 9, 21, 8, 0, 0));
    [Fact] void should_carry_end_date_time() => _event.EndDateTime.ShouldEqual(new DateTime(2026, 9, 21, 14, 0, 0));
}
