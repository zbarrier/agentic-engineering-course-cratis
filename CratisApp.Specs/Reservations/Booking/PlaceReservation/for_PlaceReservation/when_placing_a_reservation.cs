using CratisApp.Reservations.Booking;
using CratisApp.Reservations.Booking.PlaceReservation;
using PlaceReservationCommand = CratisApp.Reservations.Booking.PlaceReservation.PlaceReservation;

namespace CratisApp.Specs.Reservations.Booking.PlaceReservation.for_PlaceReservation;

public class when_placing_a_reservation : Specification
{
    PlaceReservationCommand _command;
    ReservationPlaced? _event;
    bool _isSuccess;

    void Establish() => _command = new(
        ReservationId.New(),
        RestaurantId.New(),
        "guest@example.com",
        DateTime.UtcNow.AddDays(1),
        DateTime.UtcNow.AddDays(1).AddHours(2),
        4);

    void Because()
    {
        var result = _command.Handle(existingReservation: null);
        _isSuccess = result.IsSuccess;
        result.TryGetResult(out _event);
    }

    [Fact] void should_be_successful() => _isSuccess.ShouldBeTrue();
    [Fact] void should_carry_the_restaurant_id() => _event!.RestaurantId.ShouldEqual(_command.RestaurantId);
    [Fact] void should_carry_the_email() => _event!.Email.ShouldEqual(_command.Email);
    [Fact] void should_carry_the_start() => _event!.Start.ShouldEqual(_command.Start);
    [Fact] void should_carry_the_end() => _event!.End.ShouldEqual(_command.End);
    [Fact] void should_carry_the_party_size() => _event!.PartySize.ShouldEqual(_command.PartySize);
    [Fact] void should_generate_a_six_character_code() => _event!.Code.Value.Length.ShouldEqual(6);
}
