using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Teretana.Api.Domen;
using Teretana.Api.Podaci;

namespace Teretana.PlaywrightTestovi.Infrastruktura;

[Category("API")]
public abstract class ApiTest : PlaywrightTest
{
    private const string Lozinka = "Lozinka123!";

    private HostovanaAplikacija _aplikacija = null!;

    protected IAPIRequestContext Api { get; private set; } = null!;

    [OneTimeSetUp]
    public void PokreniAplikaciju()
    {
        _aplikacija = new HostovanaAplikacija();
        _aplikacija.Pokreni();
    }

    [OneTimeTearDown]
    public async Task ZaustaviAplikaciju() => await _aplikacija.DisposeAsync();

    [SetUp]
    public async Task NapraviApiKontekst() =>
        Api = await Playwright.APIRequest.NewContextAsync(new() { BaseURL = _aplikacija.Adresa });

    [TearDown]
    public async Task ZatvoriApiKontekst() => await Api.DisposeAsync();

    protected static string NoviEmail() => $"{Guid.NewGuid():N}@test.local";

    protected static APIRequestContextOptions SaTokenom(
        PrijavljenKorisnik korisnik,
        object? telo = null,
        Dictionary<string, object>? parametri = null) => new()
        {
            Headers = new Dictionary<string, string> { ["Authorization"] = $"Bearer {korisnik.Token}" },
            DataObject = telo,
            Params = parametri,
        };

    /// <summary>
    /// Trener se upisuje direktno u bazu hostovane aplikacije (Arrange), jer API registruje samo članove.
    /// </summary>
    protected async Task<PrijavljenKorisnik> NoviTrenerAsync()
    {
        var trener = new Korisnik { Email = NoviEmail(), ImePrezime = "Api Trener", Uloga = Uloga.Trener, KreiranAt = DateTime.UtcNow };
        trener.LozinkaHash = new PasswordHasher<Korisnik>().HashPassword(trener, Lozinka);

        await using (var scope = _aplikacija.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TeretanaDbContext>();
            db.Korisnici.Add(trener);
            await db.SaveChangesAsync();
        }

        return await PrijaviAsync(trener.Email);
    }

    protected async Task<PrijavljenKorisnik> NoviClanAsync()
    {
        var email = NoviEmail();
        var registracija = await Api.PostAsync("/api/auth/registracija", new() { DataObject = new { email, imePrezime = "Api Član", lozinka = Lozinka } });
        Assert.That(registracija.Status, Is.EqualTo(201), "Registracija člana u pripremi testa nije uspela.");
        return await PrijaviAsync(email);
    }

    protected async Task<int> NoviTerminAsync(PrijavljenKorisnik trener, int kapacitet)
    {
        var pocetak = DateTimeOffset.UtcNow.AddDays(3);
        var odgovor = await Api.PostAsync("/api/termini", SaTokenom(trener, new { naziv = "Api termin", pocetak, kraj = pocetak.AddHours(1), kapacitet }));
        Assert.That(odgovor.Status, Is.EqualTo(201), "Kreiranje termina u pripremi testa nije uspelo.");
        return (await odgovor.JsonAsync())!.Value.GetProperty("id").GetInt32();
    }

    private async Task<PrijavljenKorisnik> PrijaviAsync(string email)
    {
        var prijava = await Api.PostAsync("/api/auth/prijava", new() { DataObject = new { email, lozinka = Lozinka } });
        Assert.That(prijava.Status, Is.EqualTo(200), "Prijava u pripremi testa nije uspela.");
        var telo = (await prijava.JsonAsync())!.Value;
        return new PrijavljenKorisnik(telo.GetProperty("korisnik").GetProperty("id").GetInt32(), telo.GetProperty("token").GetString()!);
    }
}

public sealed record PrijavljenKorisnik(int Id, string Token);
