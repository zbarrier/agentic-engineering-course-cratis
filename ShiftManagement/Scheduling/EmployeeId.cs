namespace CratisApp.ShiftManagement.Scheduling;

public record EmployeeId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static readonly EmployeeId NotSet = new(Guid.Empty);
    public static implicit operator Guid(EmployeeId id) => id.Value;
    public static implicit operator EmployeeId(Guid value) => new(value);
    public static EmployeeId New() => new(Guid.NewGuid());
}
