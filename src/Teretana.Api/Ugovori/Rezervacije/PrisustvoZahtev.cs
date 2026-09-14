using System.ComponentModel.DataAnnotations;

namespace Teretana.Api.Ugovori.Rezervacije;

public sealed record PrisustvoZahtev
{
    /// <summary>Da li je član došao na termin.</summary>
    /// <example>true</example>
    [Required(ErrorMessage = "Polje prisustvovao je obavezno.")]
    public bool? Prisustvovao { get; init; }
}
