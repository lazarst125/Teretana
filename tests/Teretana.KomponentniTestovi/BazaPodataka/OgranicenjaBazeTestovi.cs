using Teretana.Api.Domen;
using Teretana.KomponentniTestovi.Infrastruktura;

namespace Teretana.KomponentniTestovi.BazaPodataka;

/// <summary>
/// Pravila koja baza čuva sama, nezavisno od servisa: upisi idu direktno kroz DbContext.
/// </summary>
public sealed class OgranicenjaBazeTestovi : KomponentniTest
{
    private const int SqliteConstraintUnique = 2067;
    private const int SqliteConstraintCheck = 275;

    [Test]
    public async Task Korisnik_EmailKojiSeRazlikujeSamoUVeliciniSlova_BazaOdbija()
    {
        var postojeci = TestniEntiteti.NoviClan();
        await SaBazomAsync(db =>
        {
            db.Korisnici.Add(postojeci);
            return db.SaveChangesAsync();
        });
        var duplikat = TestniEntiteti.NoviClan();
        duplikat.Email = postojeci.Email.ToUpperInvariant();

        var greska = await UpisKojiBazaOdbijaAsync(db => db.Korisnici.Add(duplikat));

        Assert.That(greska.SqliteExtendedErrorCode, Is.EqualTo(SqliteConstraintUnique));
    }

    [Test]
    public async Task Termin_KapacitetNula_BazaOdbija()
    {
        var termin = TestniEntiteti.NoviTermin(TestniEntiteti.NoviTrener(), kapacitet: 0);

        var greska = await UpisKojiBazaOdbijaAsync(db => db.Termini.Add(termin));

        Assert.Multiple(() =>
        {
            Assert.That(greska.SqliteExtendedErrorCode, Is.EqualTo(SqliteConstraintCheck));
            Assert.That(greska.Message, Does.Contain("CK_Termini_Kapacitet"));
        });
    }

    [Test]
    public async Task Termin_BrojPotvrdjenihVeciOdKapaciteta_BazaOdbija()
    {
        var termin = TestniEntiteti.NoviTermin(TestniEntiteti.NoviTrener(), kapacitet: 2);
        termin.BrojPotvrdjenih = 3;

        var greska = await UpisKojiBazaOdbijaAsync(db => db.Termini.Add(termin));

        Assert.Multiple(() =>
        {
            Assert.That(greska.SqliteExtendedErrorCode, Is.EqualTo(SqliteConstraintCheck));
            Assert.That(greska.Message, Does.Contain("CK_Termini_BrojPotvrdjenih"));
        });
    }

    [Test]
    public async Task Termin_KrajPrePocetka_BazaOdbija()
    {
        var termin = TestniEntiteti.NoviTermin(TestniEntiteti.NoviTrener());
        termin.Kraj = termin.Pocetak.AddMinutes(-1);

        var greska = await UpisKojiBazaOdbijaAsync(db => db.Termini.Add(termin));

        Assert.Multiple(() =>
        {
            Assert.That(greska.SqliteExtendedErrorCode, Is.EqualTo(SqliteConstraintCheck));
            Assert.That(greska.Message, Does.Contain("CK_Termini_KrajPoslePocetka"));
        });
    }

    [Test]
    public async Task Rezervacija_ClanPotvrdjenIIstovremenoNaCekanjuZaIstiTermin_BazaOdbija()
    {
        var termin = TestniEntiteti.NoviTermin(TestniEntiteti.NoviTrener());
        var clan = TestniEntiteti.NoviClan();
        await SaBazomAsync(db =>
        {
            db.Rezervacije.Add(TestniEntiteti.NovaRezervacija(termin, clan, StatusRezervacije.Potvrdjena));
            return db.SaveChangesAsync();
        });

        var greska = await UpisKojiBazaOdbijaAsync(db => db.Rezervacije.Add(new Rezervacija
        {
            TerminId = termin.Id,
            ClanId = clan.Id,
            Status = StatusRezervacije.NaCekanju,
            KreiranaAt = TestniEntiteti.Sada,
        }));

        Assert.That(greska.SqliteExtendedErrorCode, Is.EqualTo(SqliteConstraintUnique));
    }

    [Test]
    public async Task Rezervacija_NovaPrijavaPosleOtkazaneZaIstiTermin_BazaDozvoljava()
    {
        var termin = TestniEntiteti.NoviTermin(TestniEntiteti.NoviTrener());
        var clan = TestniEntiteti.NoviClan();
        await SaBazomAsync(db =>
        {
            db.Rezervacije.Add(TestniEntiteti.NovaRezervacija(termin, clan, StatusRezervacije.Otkazana));
            return db.SaveChangesAsync();
        });

        await SaBazomAsync(db =>
        {
            db.Rezervacije.Add(new Rezervacija
            {
                TerminId = termin.Id,
                ClanId = clan.Id,
                Status = StatusRezervacije.Potvrdjena,
                KreiranaAt = TestniEntiteti.Sada,
            });
            return db.SaveChangesAsync();
        });

        var brojPrijava = await SaBazomAsync(db => Task.FromResult(db.Rezervacije.Count(r => r.TerminId == termin.Id && r.ClanId == clan.Id)));
        Assert.That(brojPrijava, Is.EqualTo(2));
    }
}
