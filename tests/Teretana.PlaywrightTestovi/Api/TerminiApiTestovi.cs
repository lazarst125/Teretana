using System.Text.Json;
using Teretana.PlaywrightTestovi.Infrastruktura;
using static Teretana.PlaywrightTestovi.Infrastruktura.TestniPodaci;

namespace Teretana.PlaywrightTestovi.Api;

/// <summary>
/// Pravila i status kodovi za termine pokriveni su komponentnim testovima; ovde se proverava tok između
/// dve role preko pravog HTTP-a, uključujući vreme sa zonom u query string-u.
/// </summary>
public sealed class TerminiApiTestovi : ApiTest
{
    [Test]
    public async Task TrenerKreiraIOtkazujeTermin_ClanGaNalaziFilteromIVidiOtkazivanje_PrekoHttp()
    {
        var trener = await Podaci.NoviTrenerAsync();
        var clan = await Podaci.NoviClanAsync();
        // Offset +02:00 u query string-u proverava da se znak '+' enkoduje i da server poredi UTC trenutke.
        var pocetak = new DateTimeOffset(DateTime.UtcNow.Date.AddDays(3).AddHours(10), TimeSpan.Zero).ToOffset(TimeSpan.FromHours(2));

        var kreiranje = await Api.PostAsync("/api/termini", SaTokenom(trener, new { naziv = "Api termin", pocetak, kraj = pocetak.AddHours(1), kapacitet = 5 }));
        var idTermina = (await kreiranje.JsonAsync())!.Value.GetProperty("id").GetInt32();
        var lista = await Api.GetAsync("/api/termini", SaTokenom(clan, parametri: new()
        {
            ["trenerId"] = trener.Id,
            ["od"] = pocetak.AddMinutes(-1).ToString("O"),
            ["do"] = pocetak.AddMinutes(1).ToString("O"),
            ["samoSlobodni"] = true,
        }));
        var teloListe = (await lista.JsonAsync())!.Value;
        var otkazivanje = await Api.PostAsync($"/api/termini/{idTermina}/otkazivanje", SaTokenom(trener));
        var detalj = await Api.GetAsync($"/api/termini/{idTermina}", SaTokenom(clan));
        var teloDetalja = (await detalj.JsonAsync())!.Value;

        Assert.Multiple(() =>
        {
            Assert.That(kreiranje.Status, Is.EqualTo(201));
            Assert.That(lista.Status, Is.EqualTo(200));
            Assert.That(teloListe.GetProperty("stavke").EnumerateArray().Select(t => t.GetProperty("id").GetInt32()), Is.EqualTo(new[] { idTermina }));
            Assert.That(otkazivanje.Status, Is.EqualTo(200));
            Assert.That(teloDetalja.GetProperty("status").GetString(), Is.EqualTo("Otkazan"));
            Assert.That(teloDetalja.GetProperty("slobodnaMesta").GetInt32(), Is.Zero);
            Assert.That(teloDetalja.GetProperty("pocetak").GetDateTimeOffset(), Is.EqualTo(pocetak));
            Assert.That(teloDetalja.GetProperty("mojaPrijava").ValueKind, Is.EqualTo(JsonValueKind.Null));
        });
    }
}
