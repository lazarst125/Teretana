using Teretana.Api.Infrastruktura.Autentikacija;
using Teretana.Api.Podaci;

namespace Teretana.Testovi.Zajednicko;

public static class TestnaKonfiguracija
{
    public const string JwtKljuc = "kljuc-samo-za-automatske-testove-nije-tajna-0123456789";

    public static readonly string KljucJwtKljuca = $"{JwtPodesavanja.Sekcija}:{nameof(JwtPodesavanja.Kljuc)}";

    public static Dictionary<string, string?> Osnovna(IzolovanaBaza baza) => new()
    {
        [$"ConnectionStrings:{BazaPodatakaRegistracija.NazivConnectionStringa}"] = baza.ConnectionString,
        [KljucJwtKljuca] = JwtKljuc,
    };
}
