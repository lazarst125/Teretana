using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Teretana.Api.Domen;
using Teretana.KomponentniTestovi.Infrastruktura;

namespace Teretana.KomponentniTestovi.Termini;

public sealed class IzmenaTerminaTestovi : KomponentniTest
{
    [Test]
    public async Task Izmena_SopstveniTerminBezPrijava_Vraca200SaIzmenjenimPodacima()
    {
        var trener = await NoviKorisnikUBaziAsync(Uloga.Trener);
        var termin = await NoviTerminUBaziAsync(trener);
        await PrijaviSeKaoAsync(trener);

        using var odgovor = await Klijent.PutAsJsonAsync($"/api/termini/{termin.Id}", Izmena("Pilates", kapacitet: 4));

        var izmenjen = await odgovor.Content.ReadFromJsonAsync<TerminTelo>();
        Assert.Multiple(() =>
        {
            Assert.That(odgovor.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(izmenjen?.Naziv, Is.EqualTo("Pilates"));
            Assert.That(izmenjen?.Kapacitet, Is.EqualTo(4));
            Assert.That(izmenjen?.Pocetak, Is.EqualTo(TestniEntiteti.Sada.AddDays(3)));
        });
    }

    [Test]
    public async Task Izmena_TerminSaAktivnomPrijavom_Vraca409TerminImaPrijaveIPodaciOstajuIsti()
    {
        var trener = await NoviKorisnikUBaziAsync(Uloga.Trener);
        var termin = await NoviTerminUBaziAsync(trener, naziv: "Originalni naziv");
        await NovaPrijavaUBaziAsync(termin, await NoviKorisnikUBaziAsync(Uloga.Clan), StatusRezervacije.NaCekanju);
        await PrijaviSeKaoAsync(trener);

        using var odgovor = await Klijent.PutAsJsonAsync($"/api/termini/{termin.Id}", Izmena("Novi naziv"));

        await OcekujProblemAsync(odgovor, HttpStatusCode.Conflict, "termin-ima-prijave");
        var naziv = await SaBazomAsync(db => db.Termini.Where(t => t.Id == termin.Id).Select(t => t.Naziv).SingleAsync());
        Assert.That(naziv, Is.EqualTo("Originalni naziv"));
    }

    [Test]
    public async Task Izmena_TudjiTermin_Vraca403TudjiTermin()
    {
        var termin = await NoviTerminUBaziAsync(await NoviKorisnikUBaziAsync(Uloga.Trener));
        await PrijaviSeKaoAsync(await NoviKorisnikUBaziAsync(Uloga.Trener));

        using var odgovor = await Klijent.PutAsJsonAsync($"/api/termini/{termin.Id}", Izmena("Tuđa izmena"));

        await OcekujProblemAsync(odgovor, HttpStatusCode.Forbidden, "tudji-termin");
    }

    [Test]
    public async Task Izmena_NepostojeciTermin_Vraca404TerminNePostoji()
    {
        await PrijaviSeKaoAsync(await NoviKorisnikUBaziAsync(Uloga.Trener));

        using var odgovor = await Klijent.PutAsJsonAsync("/api/termini/999999", Izmena("Bilo šta"));

        await OcekujProblemAsync(odgovor, HttpStatusCode.NotFound, "termin-ne-postoji");
    }

    private static object Izmena(string naziv, int kapacitet = 10)
    {
        var pocetak = TestniEntiteti.Sada.AddDays(3);
        return new { naziv, pocetak, kraj = pocetak.AddHours(1), kapacitet };
    }
}
