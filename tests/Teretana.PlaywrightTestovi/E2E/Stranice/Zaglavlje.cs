namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class Zaglavlje(IPage stranica)
{
    public ILocator ImeKorisnika => stranica.GetByTestId("profil-ime");

    public ILocator Uloga => stranica.GetByTestId("profil-uloga");

    public ILocator Obavestenje => stranica.GetByTestId("obavestenje");

    public ILocator NavigacijaNoviTermin => stranica.GetByTestId("nav-novi-termin");

    public async Task OdjaviSeAsync() => await stranica.GetByTestId("odjava").ClickAsync();
}
