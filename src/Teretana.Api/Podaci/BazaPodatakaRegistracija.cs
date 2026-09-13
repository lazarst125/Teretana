using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Teretana.Api.Podaci;

public static class BazaPodatakaRegistracija
{
    public const string NazivConnectionStringa = "Teretana";

    public static IServiceCollection DodajBazuPodataka(this IServiceCollection servisi) =>
        // Konfiguracija se čita pri kreiranju konteksta, a ne pri registraciji, da bi testovi mogli
        // da podmetnu sopstvenu bazu kroz konfiguraciju.
        servisi.AddDbContext<TeretanaDbContext>((provajder, opcije) => opcije.UseSqlite(
            NapraviConnectionString(
                provajder.GetRequiredService<IConfiguration>(),
                provajder.GetRequiredService<IHostEnvironment>())));

    private static string NapraviConnectionString(IConfiguration konfiguracija, IHostEnvironment okruzenje)
    {
        var connectionString = konfiguracija.GetConnectionString(NazivConnectionStringa)
            ?? throw new InvalidOperationException($"Nedostaje connection string '{NazivConnectionStringa}'.");

        var sqlite = new SqliteConnectionStringBuilder(connectionString);
        if (!Path.IsPathRooted(sqlite.DataSource))
        {
            // Relativna putanja se vezuje za folder aplikacije, a ne za folder iz kog je pokrenut `dotnet run`.
            sqlite.DataSource = Path.Combine(okruzenje.ContentRootPath, sqlite.DataSource);
        }

        return sqlite.ToString();
    }
}
