namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class PrijavaStrana(IPage stranica)
{
    public ILocator GreskaForme => stranica.GetByTestId("prijava-greska");

    public ILocator GreskaPolja(string naziv) => stranica.GetByTestId($"prijava-{naziv}-greska");

    public async Task OtvoriAsync() => await stranica.GotoAsync("/#/prijava");

    public async Task PrijaviSeAsync(string email, string lozinka)
    {
        await stranica.GetByTestId("prijava-email").FillAsync(email);
        await stranica.GetByTestId("prijava-lozinka").FillAsync(lozinka);
        await stranica.GetByTestId("prijava-potvrdi").ClickAsync();
    }
}
