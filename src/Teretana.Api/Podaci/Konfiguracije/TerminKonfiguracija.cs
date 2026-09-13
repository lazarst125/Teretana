using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teretana.Api.Domen;

namespace Teretana.Api.Podaci.Konfiguracije;

internal sealed class TerminKonfiguracija : IEntityTypeConfiguration<Termin>
{
    public void Configure(EntityTypeBuilder<Termin> builder)
    {
        builder.ToTable("Termini", tabela =>
        {
            tabela.HasCheckConstraint("CK_Termini_Kapacitet", "\"Kapacitet\" > 0");
            tabela.HasCheckConstraint("CK_Termini_BrojPotvrdjenih", "\"BrojPotvrdjenih\" >= 0 AND \"BrojPotvrdjenih\" <= \"Kapacitet\"");
            tabela.HasCheckConstraint("CK_Termini_KrajPoslePocetka", "\"Kraj\" > \"Pocetak\"");
            tabela.HasCheckConstraint("CK_Termini_Status", "\"Status\" IN ('Aktivan', 'Otkazan')");
        });

        builder.Property(t => t.Naziv).HasMaxLength(100);
        builder.Property(t => t.Opis).HasMaxLength(500);
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasOne(t => t.Trener)
            .WithMany()
            .HasForeignKey(t => t.TrenerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Pocetak);
    }
}
