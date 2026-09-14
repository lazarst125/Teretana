namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class PolazniciStrana(IPage stranica)
{
    public ILocator ImenaPotvrdjenih => stranica.GetByTestId("polaznik-ime");

    public ILocator ImenaNaCekanju => stranica.GetByTestId("cekanje-ime");

    public ILocator RedPolaznika(string imePrezime) =>
        stranica.GetByTestId("polaznik-red").Filter(new() { HasText = imePrezime });

    public ILocator Prisustvo(string imePrezime) => RedPolaznika(imePrezime).GetByTestId("polaznik-prisustvo");

    public async Task OtvoriAsync(int idTermina) => await stranica.GotoAsync($"/#/termini/{idTermina}/polaznici");

    public async Task EvidentirajDolazakAsync(string imePrezime) =>
        await RedPolaznika(imePrezime).GetByTestId("polaznik-dosao").ClickAsync();
}
