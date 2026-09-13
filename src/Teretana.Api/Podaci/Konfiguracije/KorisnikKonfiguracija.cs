using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teretana.Api.Domen;

namespace Teretana.Api.Podaci.Konfiguracije;

internal sealed class KorisnikKonfiguracija : IEntityTypeConfiguration<Korisnik>
{
    public void Configure(EntityTypeBuilder<Korisnik> builder)
    {
        builder.ToTable("Korisnici", tabela =>
            tabela.HasCheckConstraint("CK_Korisnici_Uloga", "\"Uloga\" IN ('Clan', 'Trener')"));

        // NOCASE čini jedinstveni indeks neosetljivim na velika i mala slova (Ana@x i ana@x su isti nalog).
        builder.Property(k => k.Email).HasMaxLength(254).UseCollation("NOCASE");
        builder.HasIndex(k => k.Email).IsUnique();

        builder.Property(k => k.ImePrezime).HasMaxLength(100);
        builder.Property(k => k.LozinkaHash).HasMaxLength(200);
        builder.Property(k => k.Uloga).HasConversion<string>().HasMaxLength(20);
    }
}
