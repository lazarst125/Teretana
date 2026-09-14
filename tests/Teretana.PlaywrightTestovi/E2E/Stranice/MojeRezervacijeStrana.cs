namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class MojeRezervacijeStrana(IPage stranica)
{
    public ILocator Kartica(int idRezervacije) =>
        stranica.Locator($"[data-testid='rezervacija-kartica'][data-rezervacija-id='{idRezervacije}']");

    public ILocator Status(int idRezervacije) => Kartica(idRezervacije).GetByTestId("rezervacija-status");

    public ILocator TerminOtkazan(int idRezervacije) => Kartica(idRezervacije).GetByTestId("rezervacija-termin-otkazan");

    public async Task OtvoriAsync() => await stranica.GotoAsync("/#/rezervacije");
}
