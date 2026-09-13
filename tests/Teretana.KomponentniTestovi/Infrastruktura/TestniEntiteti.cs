using Teretana.Api.Domen;

namespace Teretana.KomponentniTestovi.Infrastruktura;

/// <summary>
/// Entiteti sa ispravnim podrazumevanim vrednostima; test menja samo ono što proverava.
/// Email je jedinstven po pozivu, pa testovi ne zavise jedan od drugog.
/// </summary>
internal static class TestniEntiteti
{
    public static readonly DateTime Sada = new(2026, 9, 14, 10, 0, 0, DateTimeKind.Utc);

    public static Korisnik NoviTrener() => NoviKorisnik(Uloga.Trener);

    public static Korisnik NoviClan() => NoviKorisnik(Uloga.Clan);

    public static Termin NoviTermin(Korisnik trener, int kapacitet = 10) => new()
    {
        Trener = trener,
        Naziv = "Testni termin",
        Pocetak = Sada.AddDays(1),
        Kraj = Sada.AddDays(1).AddHours(1),
        Kapacitet = kapacitet,
        Status = StatusTermina.Aktivan,
        KreiranAt = Sada,
    };

    public static Rezervacija NovaRezervacija(Termin termin, Korisnik clan, StatusRezervacije status) => new()
    {
        Termin = termin,
        Clan = clan,
        Status = status,
        KreiranaAt = Sada,
    };

    private static Korisnik NoviKorisnik(Uloga uloga) => new()
    {
        Email = $"{Guid.NewGuid():N}@test.local",
        ImePrezime = "Testni Korisnik",
        Uloga = uloga,
        LozinkaHash = "nije-bitno-za-test",
        KreiranAt = Sada,
    };
}
