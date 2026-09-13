using Microsoft.EntityFrameworkCore;
using Teretana.Api.Podaci;
using Teretana.KomponentniTestovi.Infrastruktura;

namespace Teretana.KomponentniTestovi.BazaPodataka;

public sealed class PokretanjeUDevelopmentOkruzenjuTestovi : KomponentniTest
{
    protected override string Okruzenje => "Development";

    [Test]
    public async Task Pokretanje_UDevelopmentOkruzenju_UpisujeSveDemonstracioneTermine()
    {
        var nazivi = await SaBazomAsync(db => db.Termini.Select(t => t.Naziv).ToListAsync());

        Assert.That(nazivi, Is.EquivalentTo(new[]
        {
            PocetniPodaci.Termini.BezPrijava,
            PocetniPodaci.Termini.SlobodnaMesta,
            PocetniPodaci.Termini.Pun,
            PocetniPodaci.Termini.SaListomCekanja,
            PocetniPodaci.Termini.RokZaOtkazivanjeProsao,
            PocetniPodaci.Termini.Zavrsen,
            PocetniPodaci.Termini.Otkazan,
        }));
    }
}
