using Teretana.Api.Domen;
using Teretana.Api.Ugovori.Termini;

namespace Teretana.Api.Ugovori.Rezervacije;

public sealed record TerminUkratkoOdgovor(int Id, string Naziv, DateTime Pocetak, DateTime Kraj, StatusTermina Status, TrenerUkratko Trener);

/// <summary>
/// Rezervacija ili mesto na listi čekanja. RokZaOtkazivanje je poslednji trenutak u kome potvrđena rezervacija
/// može da se otkaže, a MozeDaSeOtkaze kaže da li bi otkazivanje sada prošlo, da klijent ne bi sam računao pravila.
/// </summary>
public sealed record RezervacijaOdgovor(
    int Id,
    StatusRezervacije Status,
    int? PozicijaNaCekanju,
    DateTime KreiranaAt,
    DateTime? PotvrdjenaAt,
    DateTime? OtkazanaAt,
    bool? Prisustvovao,
    DateTime RokZaOtkazivanje,
    bool MozeDaSeOtkaze,
    TerminUkratkoOdgovor Termin);
