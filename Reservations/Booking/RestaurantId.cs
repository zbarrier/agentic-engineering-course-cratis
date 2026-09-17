namespace CratisApp.Reservations.Booking;

public record RestaurantId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static readonly RestaurantId NotSet = new(Guid.Empty);
    public static implicit operator Guid(RestaurantId id) => id.Value;
    public static implicit operator RestaurantId(Guid value) => new(value);
    public static RestaurantId New() => new(Guid.NewGuid());
}
