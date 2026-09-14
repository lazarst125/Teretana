using System.ComponentModel.DataAnnotations;

namespace Teretana.Api.Ugovori.Autentikacija;

public sealed record PrijavaZahtev
{
    [Required(ErrorMessage = "Email je obavezan.")]
    [StringLength(254, ErrorMessage = "Email može imati najviše 254 znaka.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [StringLength(100, ErrorMessage = "Lozinka može imati najviše 100 znakova.")]
    public string Lozinka { get; init; } = string.Empty;
}
