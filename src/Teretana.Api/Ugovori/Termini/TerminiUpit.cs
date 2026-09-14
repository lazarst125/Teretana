using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Domen;
using Teretana.Api.Ugovori.Validacija;

namespace Teretana.Api.Ugovori.Termini;

/// <summary>
/// Parametri liste termina iz query string-a. Imena su eksplicitna jer MVC ključ validacione greške za
/// query parametre gradi iz imena za binding, a klijent treba da dobije isto ime koje je poslao.
/// </summary>
public sealed record TerminiUpit
{
    public const int NajvecaVelicinaStranice = 100;

    [FromQuery(Name = "stranica")]
    [Range(1, 100_000, ErrorMessage = "Stranica mora biti između 1 i 100000.")]
    public int Stranica { get; init; } = 1;

    [FromQuery(Name = "velicinaStranice")]
    [Range(1, NajvecaVelicinaStranice, ErrorMessage = "Veličina stranice mora biti između 1 i 100.")]
    public int VelicinaStranice { get; init; } = 20;

    /// <summary>Termini koji počinju u ovom trenutku ili kasnije.</summary>
    [FromQuery(Name = "od")]
    public DateTimeOffset? Od { get; init; }

    /// <summary>Termini koji počinju pre ovog trenutka.</summary>
    [FromQuery(Name = "do")]
    [Posle(nameof(Od), ErrorMessage = "Kraj opsega mora biti posle početka opsega.")]
    public DateTimeOffset? Do { get; init; }

    [FromQuery(Name = "trenerId")]
    public int? TrenerId { get; init; }

    /// <summary>Samo termini koji mogu da se rezervišu: aktivni, još nisu počeli i imaju bar jedno slobodno mesto.</summary>
    [FromQuery(Name = "samoSlobodni")]
    public bool SamoSlobodni { get; init; }

    [FromQuery(Name = "status")]
    public StatusTermina? Status { get; init; }

    [FromQuery(Name = "sortiranje")]
    [AllowedValues(
        SortiranjeTermina.PoPocetku, SortiranjeTermina.PoPocetkuOpadajuce,
        SortiranjeTermina.PoNazivu, SortiranjeTermina.PoNazivuOpadajuce,
        SortiranjeTermina.PoSlobodnimMestima, SortiranjeTermina.PoSlobodnimMestimaOpadajuce,
        ErrorMessage = "Sortiranje mora biti: pocetak, naziv ili slobodnaMesta, uz opcioni prefiks '-' za opadajući redosled.")]
    public string Sortiranje { get; init; } = SortiranjeTermina.PoPocetku;
}

public static class SortiranjeTermina
{
    public const string PoPocetku = "pocetak";
    public const string PoPocetkuOpadajuce = "-pocetak";
    public const string PoNazivu = "naziv";
    public const string PoNazivuOpadajuce = "-naziv";
    public const string PoSlobodnimMestima = "slobodnaMesta";
    public const string PoSlobodnimMestimaOpadajuce = "-slobodnaMesta";
}
