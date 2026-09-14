using Teretana.Api.Domen;

namespace Teretana.Api.Ugovori.Autentikacija;

public sealed record KorisnikOdgovor(int Id, string Email, string ImePrezime, Uloga Uloga)
{
    public static KorisnikOdgovor Iz(Korisnik korisnik) => new(korisnik.Id, korisnik.Email, korisnik.ImePrezime, korisnik.Uloga);
}

public sealed record PrijavaOdgovor(string Token, DateTime Istice, KorisnikOdgovor Korisnik);
