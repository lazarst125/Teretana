namespace Teretana.PlaywrightTestovi.E2E.Stranice;

public sealed class PocetnaStrana(IPage stranica)
{
    public ILocator StatusSistemaRezultat => stranica.GetByTestId("status-sistema-rezultat");

    private ILocator ProveriStatusSistemaDugme => stranica.GetByTestId("status-sistema-proveri");

    public async Task OtvoriAsync() => await stranica.GotoAsync("/");

    public async Task ProveriStatusSistemaAsync() => await ProveriStatusSistemaDugme.ClickAsync();
}
