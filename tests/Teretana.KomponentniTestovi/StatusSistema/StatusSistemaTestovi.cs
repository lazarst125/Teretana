using System.Net;
using System.Net.Http.Json;
using Teretana.KomponentniTestovi.Infrastruktura;

namespace Teretana.KomponentniTestovi.StatusSistema;

public sealed class StatusSistemaTestovi : KomponentniTest
{
    [Test]
    public async Task GetHealth_BezTokena_Vraca200KaoJson()
    {
        using var odgovor = await Klijent.GetAsync("/health");

        Assert.Multiple(() =>
        {
            Assert.That(odgovor.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(odgovor.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/json"));
        });
    }

    [Test]
    public async Task GetHealth_KadaJeSistemIspravan_VracaStatusHealthyISpisakProvera()
    {
        using var odgovor = await Klijent.GetAsync("/health");

        var telo = await odgovor.Content.ReadFromJsonAsync<StatusSistemaTelo>();

        Assert.That(telo, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(telo!.Status, Is.EqualTo("Healthy"));
            Assert.That(telo.Provere, Is.Not.Null);
        });
    }

    private sealed record StatusSistemaTelo(string Status, IReadOnlyList<ProveraTelo> Provere);

    private sealed record ProveraTelo(string Naziv, string Status, string? Opis);
}
