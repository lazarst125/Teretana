namespace Teretana.Api.Infrastruktura.ObradaGresaka;

internal static class ProblemDetailsPodesavanja
{
    public const string KodValidacije = "validacija";

    /// <summary>
    /// Validacione greške dobijaju isti kod kao i domenske, da bi klijent razlikovao vrstu greške
    /// po jednom polju, bez obzira na to da li je zahtev odbio model binding ili servis.
    /// </summary>
    public static void DodajKodZaValidaciju(ProblemDetailsContext kontekst)
    {
        if (kontekst.ProblemDetails is HttpValidationProblemDetails)
        {
            kontekst.ProblemDetails.Extensions.TryAdd(DomenskaGreskaHandler.PoljeKodaGreske, KodValidacije);
        }
    }
}
