namespace Teretana.PlaywrightTestovi.E2E.Stranice;

/// <summary>Raspored termina i trenerov spisak sopstvenih termina (isti ekran sa filterom po treneru).</summary>
public sealed class RasporedStrana(IPage stranica)
{
    public ILocator Kartica(int idTermina) =>
        stranica.Locator($"[data-testid='termin-kartica'][data-termin-id='{idTermina}']");

    public ILocator Stanje(int idTermina) => Kartica(idTermina).GetByTestId("termin-stanje");

    public async Task OtvoriAsync() => await stranica.GotoAsync("/#/termini");

    public async Task OtvoriMojeTermineAsync() => await stranica.GotoAsync("/#/moji-termini");

    public async Task OtvoriDetaljAsync(int idTermina) => await Kartica(idTermina).GetByTestId("termin-detalji").ClickAsync();
}
