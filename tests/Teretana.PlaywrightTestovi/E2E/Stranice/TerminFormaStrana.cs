using System.Globalization;

namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class TerminFormaStrana(IPage stranica)
{
    private const string FormatLokalnogVremena = "yyyy-MM-dd'T'HH:mm";

    public ILocator Naziv => stranica.GetByTestId("termin-forma-naziv");

    public ILocator GreskaForme => stranica.GetByTestId("termin-forma-greska");

    public ILocator GreskaPolja(string polje) => stranica.GetByTestId($"termin-forma-{polje}-greska");

    public async Task OtvoriNovuAsync() => await stranica.GotoAsync("/#/termini/novi");

    /// <summary>Početak i kraj su lokalna vremena, kao što ih korisnik unosi u polja datuma i vremena.</summary>
    public async Task PopuniAsync(string naziv, DateTime pocetak, DateTime kraj, int kapacitet)
    {
        await Naziv.FillAsync(naziv);
        await stranica.GetByTestId("termin-forma-pocetak").FillAsync(pocetak.ToString(FormatLokalnogVremena, CultureInfo.InvariantCulture));
        await stranica.GetByTestId("termin-forma-kraj").FillAsync(kraj.ToString(FormatLokalnogVremena, CultureInfo.InvariantCulture));
        await PostaviKapacitetAsync(kapacitet);
    }

    public async Task PostaviKapacitetAsync(int kapacitet) =>
        await stranica.GetByTestId("termin-forma-kapacitet").FillAsync(kapacitet.ToString(CultureInfo.InvariantCulture));

    public async Task SacuvajAsync() => await stranica.GetByTestId("termin-forma-sacuvaj").ClickAsync();
}
