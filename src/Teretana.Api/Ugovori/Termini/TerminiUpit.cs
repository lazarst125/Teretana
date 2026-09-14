using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Domen;
using Teretana.Api.Ugovori.Validacija;
using Teretana.Api.Ugovori.Zajednicko;

namespace Teretana.Api.Ugovori.Termini;

public sealed record TerminiUpit : StranicenjeUpit
{
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
