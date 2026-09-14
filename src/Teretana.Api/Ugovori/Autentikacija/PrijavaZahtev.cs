using System.ComponentModel.DataAnnotations;

namespace Teretana.Api.Ugovori.Autentikacija;

public sealed record PrijavaZahtev
{
    /// <summary>Email naloga.</summary>
    /// <example>trener1@teretana.local</example>
    [Required(ErrorMessage = "Email je obavezan.")]
    [StringLength(254, ErrorMessage = "Email može imati najviše 254 znaka.")]
    public string Email { get; init; } = string.Empty;

    /// <summary>Lozinka naloga.</summary>
    /// <example>Trener123!</example>
    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [StringLength(100, ErrorMessage = "Lozinka može imati najviše 100 znakova.")]
    public string Lozinka { get; init; } = string.Empty;
}
