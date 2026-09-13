using Microsoft.EntityFrameworkCore;
using Teretana.Api.Domen;

namespace Teretana.Api.Podaci;

public sealed class TeretanaDbContext(DbContextOptions<TeretanaDbContext> options) : DbContext(options)
{
    public DbSet<Korisnik> Korisnici => Set<Korisnik>();

    public DbSet<Termin> Termini => Set<Termin>();

    public DbSet<Rezervacija> Rezervacije => Set<Rezervacija>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeretanaDbContext).Assembly);

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeKonverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<UtcDateTimeKonverter>();
    }
}
