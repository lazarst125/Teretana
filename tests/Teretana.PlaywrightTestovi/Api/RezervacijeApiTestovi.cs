using Teretana.PlaywrightTestovi.Infrastruktura;
using static Teretana.PlaywrightTestovi.Infrastruktura.TestniPodaci;

namespace Teretana.PlaywrightTestovi.Api;

/// <summary>
/// Pojedinačna pravila rezervacija pokrivena su komponentnim testovima. Ovde se proverava ceo tok liste čekanja
/// preko pravog HTTP-a i konkurentna rezervacija preko pravog Kestrel servera, gde zahtevi stižu preko mreže.
/// </summary>
public sealed class RezervacijeApiTestovi : ApiTest
{
    [Test]
    public async Task PoslednjeMestoListaCekanjaIOtkazivanje_ClanSaListeAutomatskiDobijaMesto_PrekoHttp()
    {
        var trener = await Podaci.NoviTrenerAsync();
        var prviClan = await Podaci.NoviClanAsync();
        var drugiClan = await Podaci.NoviClanAsync();
        var idTermina = await Podaci.NoviTerminAsync(trener, kapacitet: 1);

        var rezervacija = await Api.PostAsync($"/api/termini/{idTermina}/rezervacije", SaTokenom(prviClan));
        var prijavaNaCekanju = await Api.PostAsync($"/api/termini/{idTermina}/lista-cekanja", SaTokenom(drugiClan));
        var teloRezervacije = (await rezervacija.JsonAsync())!.Value;
        var teloPrijave = (await prijavaNaCekanju.JsonAsync())!.Value;
        var otkazivanje = await Api.DeleteAsync($"/api/rezervacije/{teloRezervacije.GetProperty("id").GetInt32()}", SaTokenom(prviClan));
        var prijavaPosleOtkazivanja = (await (await Api.GetAsync($"/api/rezervacije/{teloPrijave.GetProperty("id").GetInt32()}", SaTokenom(drugiClan))).JsonAsync())!.Value;
        var polaznici = (await (await Api.GetAsync($"/api/termini/{idTermina}/polaznici", SaTokenom(trener))).JsonAsync())!.Value;

        Assert.Multiple(() =>
        {
            Assert.That(rezervacija.Status, Is.EqualTo(201));
            Assert.That(teloPrijave.GetProperty("status").GetString(), Is.EqualTo("NaCekanju"));
            Assert.That(teloPrijave.GetProperty("pozicijaNaCekanju").GetInt32(), Is.EqualTo(1));
            Assert.That(otkazivanje.Status, Is.EqualTo(204));
            Assert.That(prijavaPosleOtkazivanja.GetProperty("status").GetString(), Is.EqualTo("Potvrdjena"));
            Assert.That(polaznici.GetProperty("potvrdjeni").EnumerateArray().Select(p => p.GetProperty("clanId").GetInt32()), Is.EqualTo(new[] { drugiClan.Id }));
            Assert.That(polaznici.GetProperty("listaCekanja").GetArrayLength(), Is.Zero);
        });
    }

    [Test]
    public async Task DvaIstovremenaZahtevaZaPoslednjeMesto_PrekoKestrela_TacnoJedanDobijaMesto()
    {
        var trener = await Podaci.NoviTrenerAsync();
        var prviClan = await Podaci.NoviClanAsync();
        var drugiClan = await Podaci.NoviClanAsync();
        var idTermina = await Podaci.NoviTerminAsync(trener, kapacitet: 1);

        var odgovori = await Task.WhenAll(
            Api.PostAsync($"/api/termini/{idTermina}/rezervacije", SaTokenom(prviClan)),
            Api.PostAsync($"/api/termini/{idTermina}/rezervacije", SaTokenom(drugiClan)));

        Assert.That(odgovori.Select(o => o.Status), Is.EquivalentTo(new[] { 201, 409 }));
    }
}
