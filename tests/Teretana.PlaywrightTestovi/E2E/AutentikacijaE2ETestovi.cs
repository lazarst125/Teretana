using System.Text.RegularExpressions;
using Teretana.PlaywrightTestovi.E2E.Stranice;
using Teretana.PlaywrightTestovi.Infrastruktura;

namespace Teretana.PlaywrightTestovi.E2E;

public sealed class AutentikacijaE2ETestovi : E2ETest
{
    [Test]
    public async Task Prijava_PogresnaLozinka_PrikazujePorukuServeraIOstajeNaPrijavi()
    {
        var clan = await Podaci.NoviClanAsync();
        var prijava = new PrijavaStrana(Page);
        await prijava.OtvoriAsync();

        await prijava.PrijaviSeAsync(clan.Email, "PogresnaLozinka1!");

        await Expect(prijava.GreskaForme).ToHaveTextAsync("Email ili lozinka nisu ispravni.");
        await Expect(Page).ToHaveURLAsync(new Regex("#/prijava$"));
    }

    [Test]
    public async Task ZasticenaAdresaBezPrijave_PosleUspesnePrijave_OtvaraTrazenuStranu()
    {
        var clan = await Podaci.NoviClanAsync("Ana Povratak");
        await Page.GotoAsync("/#/rezervacije");
        await Expect(Page).ToHaveURLAsync(new Regex("#/prijava\\?povratak=%2Frezervacije$"));

        await new PrijavaStrana(Page).PrijaviSeAsync(clan.Email, TestniPodaci.Lozinka);

        await Expect(Page).ToHaveURLAsync(new Regex("#/rezervacije$"));
        await Expect(new Zaglavlje(Page).ImeKorisnika).ToHaveTextAsync("Ana Povratak");
    }

    [Test]
    public async Task ClanNaTrenerskojAdresi_VidiNematePristupINemaTrenerskuNavigaciju()
    {
        await PrijaviSeAsync(await Podaci.NoviClanAsync());

        await Page.GotoAsync("/#/termini/novi");

        await Expect(Page.GetByTestId("nemate-pristup")).ToBeVisibleAsync();
        await Expect(new Zaglavlje(Page).NavigacijaNoviTermin).ToHaveCountAsync(0);
    }

    [Test]
    public async Task Odjava_PrijavljenClan_VracaNaPrijavuIZasticeneStraneTraziPonovnuPrijavu()
    {
        await PrijaviSeAsync(await Podaci.NoviClanAsync());
        var zaglavlje = new Zaglavlje(Page);

        await zaglavlje.OdjaviSeAsync();

        await Expect(zaglavlje.Obavestenje).ToHaveTextAsync("Odjavljeni ste.");
        await Page.GotoAsync("/#/rezervacije");
        await Expect(Page).ToHaveURLAsync(new Regex("#/prijava\\?povratak=%2Frezervacije$"));
    }
}
