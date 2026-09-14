namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class TerminDetaljStrana(IPage stranica)
{
    private ILocator Detalj => stranica.GetByTestId("termin-detalj");

    public ILocator Naziv => stranica.GetByTestId("termin-naziv");

    public ILocator Stanje => Detalj.GetByTestId("termin-stanje");

    public ILocator Kapacitet => stranica.GetByTestId("termin-kapacitet");

    public ILocator SlobodnaMesta => stranica.GetByTestId("termin-slobodna-mesta");

    public ILocator MojaPrijava => stranica.GetByTestId("termin-moja-prijava");

    public ILocator Napomena => stranica.GetByTestId("termin-napomena");

    public ILocator GreskaAkcije => stranica.GetByTestId("termin-akcija-greska");

    public ILocator StanjeGreske => stranica.GetByTestId("termin-greska");

    public ILocator DugmePokusajPonovo => stranica.GetByTestId("termin-ponovo");

    public ILocator DugmeRezervisi => stranica.GetByTestId("termin-rezervisi");

    public ILocator DugmeListaCekanja => stranica.GetByTestId("termin-lista-cekanja");

    public async Task OtvoriAsync(int idTermina) => await stranica.GotoAsync($"/#/termini/{idTermina}");

    public async Task RezervisiAsync() => await DugmeRezervisi.ClickAsync();

    public async Task PrijaviNaListuCekanjaAsync() => await DugmeListaCekanja.ClickAsync();

    public async Task OtvoriIzmenuAsync() => await stranica.GetByTestId("termin-izmeni").ClickAsync();

    public async Task OtvoriPolazniceAsync() => await stranica.GetByTestId("termin-polaznici").ClickAsync();

    public async Task OtkaziTerminAsync()
    {
        await stranica.GetByTestId("termin-otkazi").ClickAsync();
        await new DijalogPotvrde(stranica).PotvrdiAsync();
    }

    public async Task ObrisiAsync()
    {
        await stranica.GetByTestId("termin-obrisi").ClickAsync();
        await new DijalogPotvrde(stranica).PotvrdiAsync();
    }
}
