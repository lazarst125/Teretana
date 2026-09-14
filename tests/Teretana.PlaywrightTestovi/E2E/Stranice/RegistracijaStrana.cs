namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class RegistracijaStrana(IPage stranica)
{
    public ILocator GreskaForme => stranica.GetByTestId("registracija-greska");

    public async Task OtvoriAsync() => await stranica.GotoAsync("/#/registracija");

    public async Task RegistrujAsync(string imePrezime, string email, string lozinka)
    {
        await stranica.GetByTestId("registracija-ime-prezime").FillAsync(imePrezime);
        await stranica.GetByTestId("registracija-email").FillAsync(email);
        await stranica.GetByTestId("registracija-lozinka").FillAsync(lozinka);
        await stranica.GetByTestId("registracija-potvrdi").ClickAsync();
    }
}
