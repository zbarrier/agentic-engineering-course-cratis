namespace CratisApp.ShiftManagement.Scheduling.CreateShift;

using CratisApp.ShiftManagement.Scheduling;

[Command]
public record CreateShift(
    string Name,
    string Type,
    bool Recurring,
    string FromDay,
    string ToDay,
    DateTime StartDateTime,
    DateTime EndDateTime)
{
    public (ShiftId, ShiftCreated) Handle()
    {
        var id = ShiftId.New();
        return (id, new ShiftCreated(id, Name, Type, Recurring, FromDay, ToDay, StartDateTime, EndDateTime));
    }
}

[EventType]
public record ShiftCreated(
    ShiftId ShiftId,
    string Name,
    string Type,
    bool Recurring,
    string FromDay,
    string ToDay,
    DateTime StartDateTime,
    DateTime EndDateTime);

public class CreateShiftValidator : CommandValidator<CreateShift>
{
    static readonly string[] WeekOrder =
    [
        "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
    ];

    public CreateShiftValidator()
    {
        RuleFor(command => command.EndDateTime)
            .GreaterThan(command => command.StartDateTime)
            .WithMessage("endDateTime must be after startDateTime");

        RuleFor(command => command.FromDay)
            .Must((command, fromDay) => DayIndex(fromDay) <= DayIndex(command.ToDay))
            .WithMessage("fromDay must not be later in the week than toDay");
    }

    static int DayIndex(string day) => Array.IndexOf(WeekOrder, day);
}
