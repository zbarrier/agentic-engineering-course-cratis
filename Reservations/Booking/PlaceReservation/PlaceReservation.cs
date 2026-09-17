namespace CratisApp.Reservations.Booking.PlaceReservation;

using Cratis.Chronicle.Keys;
using Cratis.Monads;
using CratisApp.Reservations.Booking;

[Command]
public record PlaceReservation(
    [Key] ReservationId Id,
    RestaurantId RestaurantId,
    string Email,
    DateTime Start,
    DateTime End,
    int PartySize)
{
    public Result<ReservationPlaced, PlaceReservationError> Handle(ExistingReservation? existingReservation) =>
        existingReservation is not null
            ? PlaceReservationError.ReservationAlreadyExists
            : new ReservationPlaced(Id, RestaurantId, Email, Start, End, PartySize, ReservationCode.Generate());
}

public enum PlaceReservationError
{
    ReservationAlreadyExists
}

[EventType]
public record ReservationPlaced(
    ReservationId Id,
    RestaurantId RestaurantId,
    string Email,
    DateTime Start,
    DateTime End,
    int PartySize,
    ReservationCode Code);

[ReadModel]
[FromEvent<ReservationPlaced>]
public record ExistingReservation(ReservationId Id);

public class PlaceReservationValidator : CommandValidator<PlaceReservation>
{
    public PlaceReservationValidator()
    {
        RuleFor(command => command.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(command => command.PartySize).GreaterThanOrEqualTo(1).WithMessage("At least 1 person is required");
        RuleFor(command => command.End).GreaterThan(command => command.Start).WithMessage("End must be after start");
        RuleFor(command => command.Start).GreaterThan(_ => DateTime.UtcNow).WithMessage("Reservation cannot be placed in the past");
    }
}
