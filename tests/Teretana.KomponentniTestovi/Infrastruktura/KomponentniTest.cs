using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teretana.Api.Domen;
using Teretana.Api.Podaci;

namespace Teretana.KomponentniTestovi.Infrastruktura;

/// <summary>
/// Svaki test dobija sopstvenu instancu aplikacije i sopstvenu SQLite bazu, pa testovi mogu da se
/// izvršavaju paralelno i bilo kojim redosledom.
/// </summary>
[Parallelizable(ParallelScope.All)]
[Category("Komponentni")]
public abstract class KomponentniTest
{
    protected const string TestnaLozinka = "Lozinka123!";

    protected TeretanaAplikacija Aplikacija { get; private set; } = null!;

    protected HttpClient Klijent { get; private set; } = null!;

    protected virtual string Okruzenje => "Testing";

    [SetUp]
    public void PokreniAplikaciju()
    {
        Aplikacija = new TeretanaAplikacija(Okruzenje, PodesiKonfiguraciju, PodesiTestneServise);
        Klijent = Aplikacija.CreateClient();
    }

    [TearDown]
    public async Task ZaustaviAplikaciju()
    {
        Klijent.Dispose();
        await Aplikacija.DisposeAsync();
    }

    protected static string NoviEmail() => $"{Guid.NewGuid():N}@test.local";

    protected static async Task<JsonElement> ProblemIzOdgovoraAsync(HttpResponseMessage odgovor)
    {
        Assert.That(odgovor.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/problem+json"));
        return await odgovor.Content.ReadFromJsonAsync<JsonElement>();
    }

    protected virtual void PodesiKonfiguraciju(IDictionary<string, string?> konfiguracija)
    {
    }

    protected virtual void PodesiTestneServise(IServiceCollection servisi)
    {
    }

    protected async Task SaBazomAsync(Func<TeretanaDbContext, Task> akcija)
    {
        await using var scope = Aplikacija.Services.CreateAsyncScope();
        await akcija(scope.ServiceProvider.GetRequiredService<TeretanaDbContext>());
    }

    protected async Task<T> SaBazomAsync<T>(Func<TeretanaDbContext, Task<T>> upit)
    {
        await using var scope = Aplikacija.Services.CreateAsyncScope();
        return await upit(scope.ServiceProvider.GetRequiredService<TeretanaDbContext>());
    }

    /// <summary>
    /// Izvršava izmenu direktno nad bazom, mimo servisa, i vraća grešku kojom je baza odbila upis.
    /// </summary>
    protected async Task<SqliteException> UpisKojiBazaOdbijaAsync(Action<TeretanaDbContext> izmena)
    {
        await using var scope = Aplikacija.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<TeretanaDbContext>();
        izmena(db);

        var izuzetak = Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        return izuzetak?.InnerException as SqliteException
            ?? throw new AssertionException($"Očekivana je SQLite greška, a dobijeno je: {izuzetak?.InnerException}");
    }

    /// <summary>
    /// Upisuje korisnika sa lozinkom <see cref="TestnaLozinka"/> direktno u bazu; tako nastaju i treneri,
    /// jer registracija kroz API pravi samo članove.
    /// </summary>
    protected async Task<Korisnik> NoviKorisnikUBaziAsync(Uloga uloga)
    {
        var korisnik = uloga == Uloga.Trener ? TestniEntiteti.NoviTrener() : TestniEntiteti.NoviClan();
        korisnik.LozinkaHash = new PasswordHasher<Korisnik>().HashPassword(korisnik, TestnaLozinka);
        await SaBazomAsync(db =>
        {
            db.Korisnici.Add(korisnik);
            return db.SaveChangesAsync();
        });
        return korisnik;
    }

    protected async Task PrijaviSeKaoAsync(Korisnik korisnik)
    {
        using var odgovor = await Klijent.PostAsJsonAsync("/api/auth/prijava", new { email = korisnik.Email, lozinka = TestnaLozinka });
        odgovor.EnsureSuccessStatusCode();
        var telo = await odgovor.Content.ReadFromJsonAsync<JsonElement>();
        Klijent.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", telo.GetProperty("token").GetString());
    }
}
