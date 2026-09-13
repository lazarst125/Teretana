using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    protected TeretanaAplikacija Aplikacija { get; private set; } = null!;

    protected HttpClient Klijent { get; private set; } = null!;

    protected virtual string Okruzenje => "Testing";

    [SetUp]
    public void PokreniAplikaciju()
    {
        Aplikacija = new TeretanaAplikacija(Okruzenje, PodesiTestneServise);
        Klijent = Aplikacija.CreateClient();
    }

    [TearDown]
    public async Task ZaustaviAplikaciju()
    {
        Klijent.Dispose();
        await Aplikacija.DisposeAsync();
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
}
