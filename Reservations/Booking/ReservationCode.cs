using System.Security.Cryptography;

namespace CratisApp.Reservations.Booking;

public record ReservationCode(string Value) : ConceptAs<string>(Value)
{
    const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    const int Length = 6;

    public static readonly ReservationCode NotSet = new(string.Empty);
    public static implicit operator string(ReservationCode code) => code.Value;
    public static implicit operator ReservationCode(string value) => new(value);

    public static ReservationCode Generate()
    {
        var characters = new char[Length];
        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        }

        return new(new string(characters));
    }
}
