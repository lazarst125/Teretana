namespace Teretana.Api.Domen;

public enum VrstaGreske
{
    NijeAutorizovan,
    Zabranjeno,
    NijePronadjeno,
    Konflikt,
    PoslovnoPravilo,
}

/// <summary>
/// Očekivano odbijanje zahteva po poslovnom pravilu. Servisi je bacaju, a obrada grešaka je na jednom
/// mestu prevodi u ProblemDetails sa odgovarajućim statusom i stabilnim kodom greške.
/// </summary>
public sealed class DomenskaGreska(VrstaGreske vrsta, string kod, string poruka) : Exception(poruka)
{
    public VrstaGreske Vrsta { get; } = vrsta;

    public string Kod { get; } = kod;
}
