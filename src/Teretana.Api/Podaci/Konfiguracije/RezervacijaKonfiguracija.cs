using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teretana.Api.Domen;

namespace Teretana.Api.Podaci.Konfiguracije;

internal sealed class RezervacijaKonfiguracija : IEntityTypeConfiguration<Rezervacija>
{
    public void Configure(EntityTypeBuilder<Rezervacija> builder)
    {
        builder.ToTable("Rezervacije", tabela =>
            tabela.HasCheckConstraint("CK_Rezervacije_Status", "\"Status\" IN ('Potvrdjena', 'NaCekanju', 'Otkazana')"));

        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);

        // Restrict: termin ili član sa prijavama ne može da se obriše ni kad se servis zaobiđe.
        builder.HasOne(r => r.Termin)
            .WithMany(t => t.Rezervacije)
            .HasForeignKey(r => r.TerminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Clan)
            .WithMany()
            .HasForeignKey(r => r.ClanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Otkazane prijave su van indeksa, pa član posle otkazivanja može ponovo da se prijavi.
        builder.HasIndex(r => new { r.TerminId, r.ClanId })
            .IsUnique()
            .HasFilter("\"Status\" IN ('Potvrdjena', 'NaCekanju')")
            .HasDatabaseName("UX_Rezervacije_AktivnaPrijava");

        builder.HasIndex(r => new { r.TerminId, r.Status, r.KreiranaAt, r.Id })
            .HasDatabaseName("IX_Rezervacije_RedCekanja");
    }
}
