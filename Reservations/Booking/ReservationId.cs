namespace CratisApp.Reservations.Booking;

public record ReservationId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static readonly ReservationId NotSet = new(Guid.Empty);
    public static implicit operator Guid(ReservationId id) => id.Value;
    public static implicit operator ReservationId(Guid value) => new(value);
    public static implicit operator EventSourceId(ReservationId id) => new(id.Value.ToString());
    public static ReservationId New() => new(Guid.NewGuid());
}
