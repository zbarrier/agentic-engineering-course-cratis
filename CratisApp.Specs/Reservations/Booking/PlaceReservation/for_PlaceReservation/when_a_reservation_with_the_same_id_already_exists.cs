using CratisApp.Reservations.Booking;
using CratisApp.Reservations.Booking.PlaceReservation;
using PlaceReservationCommand = CratisApp.Reservations.Booking.PlaceReservation.PlaceReservation;

namespace CratisApp.Specs.Reservations.Booking.PlaceReservation.for_PlaceReservation;

public class when_a_reservation_with_the_same_id_already_exists : Specification
{
    PlaceReservationCommand _command;
    ExistingReservation _existingReservation;
    bool _isSuccess;
    PlaceReservationError _error;

    void Establish()
    {
        var id = ReservationId.New();
        _command = new(
            id,
            RestaurantId.New(),
            "other@example.com",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(2),
            4);
        _existingReservation = new(id);
    }

    void Because()
    {
        var result = _command.Handle(_existingReservation);
        _isSuccess = result.IsSuccess;
        result.TryGetError(out _error);
    }

    [Fact] void should_not_be_successful() => _isSuccess.ShouldBeFalse();
    [Fact] void should_fail_because_the_reservation_already_exists() => _error.ShouldEqual(PlaceReservationError.ReservationAlreadyExists);
}
