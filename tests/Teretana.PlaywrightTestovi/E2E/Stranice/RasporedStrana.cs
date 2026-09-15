using System.Globalization;

namespace Teretana.PlaywrightTestovi.E2E.Stranice;

/// <summary>Raspored termina i trenerov spisak sopstvenih termina (isti ekran sa filterom po treneru).</summary>
public sealed class RasporedStrana(IPage stranica)
{
    public ILocator Kartice => stranica.GetByTestId("termin-kartica");

    public ILocator NaziviTermina => Kartice.GetByTestId("termin-detalji");

    public ILocator InfoStranice => stranica.GetByTestId("termini-stranicenje-info");

    public ILocator Kartica(int idTermina) =>
        stranica.Locator($"[data-testid='termin-kartica'][data-termin-id='{idTermina}']");

    public ILocator Stanje(int idTermina) => Kartica(idTermina).GetByTestId("termin-stanje");

    public async Task OtvoriAsync() => await stranica.GotoAsync("/#/termini");

    public async Task OtvoriMojeTermineAsync() => await stranica.GotoAsync("/#/moji-termini");

    public async Task OtvoriDetaljAsync(int idTermina) => await Kartica(idTermina).GetByTestId("termin-detalji").ClickAsync();

    /// <summary>Postavlja samo zadate filtere (ostali zadržavaju trenutnu vrednost) i primenjuje ih.</summary>
    public async Task FiltrirajAsync(int? idTrenera = null, bool samoSlobodni = false, string? sortiranje = null, int? velicinaStranice = null)
    {
        if (idTrenera is { } trener)
        {
            await stranica.GetByTestId("filter-trener").SelectOptionAsync(trener.ToString(CultureInfo.InvariantCulture));
        }

        if (samoSlobodni)
        {
            await stranica.GetByTestId("filter-samo-slobodni").CheckAsync();
        }

        if (sortiranje is not null)
        {
            await stranica.GetByTestId("filter-sortiranje").SelectOptionAsync(sortiranje);
        }

        if (velicinaStranice is { } velicina)
        {
            await stranica.GetByTestId("filter-velicina-stranice").SelectOptionAsync(velicina.ToString(CultureInfo.InvariantCulture));
        }

        await stranica.GetByTestId("filter-primeni").ClickAsync();
    }

    public async Task SledecaStranaAsync() => await stranica.GetByTestId("termini-stranicenje-sledeca").ClickAsync();
}
