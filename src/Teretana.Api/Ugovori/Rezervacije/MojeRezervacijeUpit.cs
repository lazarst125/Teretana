using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Domen;
using Teretana.Api.Ugovori.Zajednicko;

namespace Teretana.Api.Ugovori.Rezervacije;

public sealed record MojeRezervacijeUpit : StranicenjeUpit
{
    [FromQuery(Name = "status")]
    public StatusRezervacije? Status { get; init; }

    [FromQuery(Name = "sortiranje")]
    [AllowedValues(
        SortiranjeRezervacija.PoPocetkuTermina, SortiranjeRezervacija.PoPocetkuTerminaOpadajuce,
        ErrorMessage = "Sortiranje mora biti: pocetak ili -pocetak.")]
    public string Sortiranje { get; init; } = SortiranjeRezervacija.PoPocetkuTermina;
}

public static class SortiranjeRezervacija
{
    public const string PoPocetkuTermina = "pocetak";
    public const string PoPocetkuTerminaOpadajuce = "-pocetak";
}
