namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class RezervacijaDetaljStrana(IPage stranica)
{
    public ILocator Status => stranica.GetByTestId("rezervacija-detalj").GetByTestId("rezervacija-status");

    public ILocator DugmeOtkazi => stranica.GetByTestId("rezervacija-otkazi");

    public ILocator OtkazivanjeNedostupno => stranica.GetByTestId("rezervacija-otkazivanje-nedostupno");

    public async Task OtvoriAsync(int idRezervacije) => await stranica.GotoAsync($"/#/rezervacije/{idRezervacije}");

    public async Task OtkaziAsync()
    {
        await DugmeOtkazi.ClickAsync();
        await new DijalogPotvrde(stranica).PotvrdiAsync();
    }
}
