using System.ComponentModel.DataAnnotations;

namespace Teretana.Api.Ugovori.Rezervacije;

public sealed record PrisustvoZahtev
{
    [Required(ErrorMessage = "Polje prisustvovao je obavezno.")]
    public bool? Prisustvovao { get; init; }
}
