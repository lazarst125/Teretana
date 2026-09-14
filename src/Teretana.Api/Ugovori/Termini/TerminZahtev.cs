using System.ComponentModel.DataAnnotations;
using Teretana.Api.Ugovori.Validacija;

namespace Teretana.Api.Ugovori.Termini;

public sealed record TerminZahtev
{
    /// <summary>Naziv termina.</summary>
    /// <example>Joga za početnike</example>
    [Required(ErrorMessage = "Naziv je obavezan.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Naziv mora imati između 2 i 100 znakova.")]
    public string Naziv { get; init; } = string.Empty;

    /// <summary>Opcioni opis termina.</summary>
    /// <example>Lagan uvod u disanje i osnovne položaje.</example>
    [StringLength(500, ErrorMessage = "Opis može imati najviše 500 znakova.")]
    public string? Opis { get; init; }

    /// <summary>Početak termina sa vremenskom zonom; mora biti u budućnosti i čuva se kao UTC.</summary>
    /// <example>2026-10-01T18:00:00+02:00</example>
    [Required(ErrorMessage = "Početak je obavezan.")]
    public DateTimeOffset? Pocetak { get; init; }

    /// <summary>Kraj termina sa vremenskom zonom; mora biti posle početka.</summary>
    /// <example>2026-10-01T19:00:00+02:00</example>
    [Required(ErrorMessage = "Kraj je obavezan.")]
    [Posle(nameof(Pocetak), ErrorMessage = "Kraj mora biti posle početka.")]
    public DateTimeOffset? Kraj { get; init; }

    /// <summary>Najveći broj potvrđenih rezervacija, od 1 do 100.</summary>
    /// <example>12</example>
    [Range(1, 100, ErrorMessage = "Kapacitet mora biti između 1 i 100.")]
    public int Kapacitet { get; init; }
}
