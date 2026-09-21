namespace CratisApp.ShiftManagement.Scheduling;

public record ShiftId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static readonly ShiftId NotSet = new(Guid.Empty);
    public static implicit operator Guid(ShiftId id) => id.Value;
    public static implicit operator ShiftId(Guid value) => new(value);
    public static implicit operator EventSourceId(ShiftId id) => new(id.Value.ToString());
    public static ShiftId New() => new(Guid.NewGuid());
}
