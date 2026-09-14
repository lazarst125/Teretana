using System.ComponentModel.DataAnnotations;
using Teretana.Api.Ugovori.Validacija;

namespace Teretana.Api.Ugovori.Termini;

public sealed record TerminZahtev
{
    [Required(ErrorMessage = "Naziv je obavezan.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Naziv mora imati između 2 i 100 znakova.")]
    public string Naziv { get; init; } = string.Empty;

    [StringLength(500, ErrorMessage = "Opis može imati najviše 500 znakova.")]
    public string? Opis { get; init; }

    /// <summary>Vreme sa zonom (npr. 2026-09-20T18:00:00+02:00); čuva se kao UTC.</summary>
    [Required(ErrorMessage = "Početak je obavezan.")]
    public DateTimeOffset? Pocetak { get; init; }

    [Required(ErrorMessage = "Kraj je obavezan.")]
    [Posle(nameof(Pocetak), ErrorMessage = "Kraj mora biti posle početka.")]
    public DateTimeOffset? Kraj { get; init; }

    [Range(1, 100, ErrorMessage = "Kapacitet mora biti između 1 i 100.")]
    public int Kapacitet { get; init; }
}
