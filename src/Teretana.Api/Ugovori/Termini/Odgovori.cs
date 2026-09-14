using Teretana.Api.Domen;

namespace Teretana.Api.Ugovori.Termini;

public sealed record TrenerUkratko(int Id, string ImePrezime);

public sealed record TerminStavkaOdgovor(
    int Id,
    string Naziv,
    DateTime Pocetak,
    DateTime Kraj,
    int Kapacitet,
    int BrojPotvrdjenih,
    int SlobodnaMesta,
    int BrojNaCekanju,
    StatusTermina Status,
    TrenerUkratko Trener);

public sealed record TerminDetaljOdgovor(
    int Id,
    string Naziv,
    string? Opis,
    DateTime Pocetak,
    DateTime Kraj,
    int Kapacitet,
    int BrojPotvrdjenih,
    int SlobodnaMesta,
    int BrojNaCekanju,
    StatusTermina Status,
    TrenerUkratko Trener,
    MojaPrijavaOdgovor? MojaPrijava);

/// <summary>Aktivna prijava člana koji gleda termin; za trenera i za člana bez prijave je null.</summary>
public sealed record MojaPrijavaOdgovor(int RezervacijaId, StatusRezervacije Status, int? PozicijaNaCekanju);

public sealed record PolazniciOdgovor(
    int TerminId,
    IReadOnlyList<PotvrdjeniPolaznikOdgovor> Potvrdjeni,
    IReadOnlyList<PolaznikNaCekanjuOdgovor> ListaCekanja);

public sealed record PotvrdjeniPolaznikOdgovor(int RezervacijaId, int ClanId, string ImePrezime, string Email, DateTime? PotvrdjenaAt, bool? Prisustvovao);

public sealed record PolaznikNaCekanjuOdgovor(int RezervacijaId, int ClanId, string ImePrezime, string Email, int Pozicija, DateTime PrijavljenAt);
