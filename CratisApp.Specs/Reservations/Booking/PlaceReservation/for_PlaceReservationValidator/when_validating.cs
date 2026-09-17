using CratisApp.Reservations.Booking;
using CratisApp.Reservations.Booking.PlaceReservation;
using PlaceReservationCommand = CratisApp.Reservations.Booking.PlaceReservation.PlaceReservation;

namespace CratisApp.Specs.Reservations.Booking.PlaceReservation.for_PlaceReservationValidator;

public class when_all_fields_are_valid : Specification
{
    PlaceReservationValidator _validator;
    FluentValidation.Results.ValidationResult _result;

    void Establish() => _validator = new();

    void Because() => _result = _validator.Validate(new PlaceReservationCommand(
        ReservationId.New(),
        RestaurantId.New(),
        "guest@example.com",
        DateTime.UtcNow.AddDays(1),
        DateTime.UtcNow.AddDays(1).AddHours(2),
        4));

    [Fact] void should_be_valid() => _result.IsValid.ShouldBeTrue();
}

public class when_party_size_is_less_than_one : Specification
{
    PlaceReservationValidator _validator;
    FluentValidation.Results.ValidationResult _result;

    void Establish() => _validator = new();

    void Because() => _result = _validator.Validate(new PlaceReservationCommand(
        ReservationId.New(),
        RestaurantId.New(),
        "guest@example.com",
        DateTime.UtcNow.AddDays(1),
        DateTime.UtcNow.AddDays(1).AddHours(2),
        0));

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
}

public class when_end_is_before_start : Specification
{
    PlaceReservationValidator _validator;
    FluentValidation.Results.ValidationResult _result;

    void Establish() => _validator = new();

    void Because() => _result = _validator.Validate(new PlaceReservationCommand(
        ReservationId.New(),
        RestaurantId.New(),
        "guest@example.com",
        DateTime.UtcNow.AddDays(1).AddHours(2),
        DateTime.UtcNow.AddDays(1),
        4));

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
}

public class when_start_is_in_the_past : Specification
{
    PlaceReservationValidator _validator;
    FluentValidation.Results.ValidationResult _result;

    void Establish() => _validator = new();

    void Because() => _result = _validator.Validate(new PlaceReservationCommand(
        ReservationId.New(),
        RestaurantId.New(),
        "guest@example.com",
        DateTime.UtcNow.AddDays(-1),
        DateTime.UtcNow.AddDays(-1).AddHours(2),
        4));

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
}
