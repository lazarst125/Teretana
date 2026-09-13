namespace Teretana.PlaywrightTestovi.Infrastruktura;

[Category("API")]
public abstract class ApiTest : PlaywrightTest
{
    private HostovanaAplikacija _aplikacija = null!;

    protected IAPIRequestContext Api { get; private set; } = null!;

    [OneTimeSetUp]
    public void PokreniAplikaciju()
    {
        _aplikacija = new HostovanaAplikacija();
        _aplikacija.Pokreni();
    }

    [OneTimeTearDown]
    public async Task ZaustaviAplikaciju() => await _aplikacija.DisposeAsync();

    [SetUp]
    public async Task NapraviApiKontekst() =>
        Api = await Playwright.APIRequest.NewContextAsync(new() { BaseURL = _aplikacija.Adresa });

    [TearDown]
    public async Task ZatvoriApiKontekst() => await Api.DisposeAsync();
}
