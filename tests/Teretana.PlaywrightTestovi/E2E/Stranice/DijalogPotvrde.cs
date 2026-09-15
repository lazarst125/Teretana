namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class DijalogPotvrde(IPage stranica)
{
    public ILocator Dijalog => stranica.GetByTestId("potvrda-dijalog");

    public async Task PotvrdiAsync() => await stranica.GetByTestId("potvrda-da").ClickAsync();

    public async Task OdustaniAsync() => await stranica.GetByTestId("potvrda-ne").ClickAsync();

    public async Task ZatvoriTasteromEscapeAsync() => await stranica.Keyboard.PressAsync("Escape");
}
