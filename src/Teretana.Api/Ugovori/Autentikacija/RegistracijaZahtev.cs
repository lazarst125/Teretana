using System.ComponentModel.DataAnnotations;

namespace Teretana.Api.Ugovori.Autentikacija;

public sealed record RegistracijaZahtev
{
    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Email nije ispravan.")]
    [StringLength(254, ErrorMessage = "Email može imati najviše 254 znaka.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Ime i prezime su obavezni.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ime i prezime moraju imati između 2 i 100 znakova.")]
    public string ImePrezime { get; init; } = string.Empty;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Lozinka mora imati između 8 i 100 znakova.")]
    public string Lozinka { get; init; } = string.Empty;
}
