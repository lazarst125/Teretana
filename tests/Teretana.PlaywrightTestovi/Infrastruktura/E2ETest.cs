using NUnit.Framework.Interfaces;

namespace Teretana.PlaywrightTestovi.Infrastruktura;

/// <summary>
/// Osnova E2E testova: aplikacija po klasi, novi browser context po testu, a trace, screenshot
/// i video se čuvaju samo kad test padne.
/// </summary>
[Category("E2E")]
public abstract class E2ETest : PageTest
{
    private static readonly string FolderZaVideo = Path.Combine(Path.GetTempPath(), "teretana-playwright-video");

    private HostovanaAplikacija _aplikacija = null!;

    [OneTimeSetUp]
    public void PokreniAplikaciju()
    {
        _aplikacija = new HostovanaAplikacija();
        _aplikacija.Pokreni();
    }

    [OneTimeTearDown]
    public async Task ZaustaviAplikaciju() => await _aplikacija.DisposeAsync();

    public override BrowserNewContextOptions ContextOptions() => new()
    {
        BaseURL = _aplikacija.Adresa,
        RecordVideoDir = FolderZaVideo,
    };

    [SetUp]
    public async Task PokreniTrace() =>
        await Context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true, Sources = true });

    [TearDown]
    public async Task SacuvajArtefakteAkoJeTestPao()
    {
        var testJePao = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;
        var folder = FolderArtefakata();

        if (testJePao)
        {
            await Page.ScreenshotAsync(new() { Path = Path.Combine(folder, "screenshot.png"), FullPage = true });
            await Context.Tracing.StopAsync(new() { Path = Path.Combine(folder, "trace.zip") });
        }
        else
        {
            await Context.Tracing.StopAsync();
        }

        var video = Page.Video;
        await Context.CloseAsync();
        if (video is null)
        {
            return;
        }

        if (testJePao)
        {
            await video.SaveAsAsync(Path.Combine(folder, "video.webm"));
        }

        await video.DeleteAsync();
    }

    private static string FolderArtefakata()
    {
        var imeTesta = string.Concat(TestContext.CurrentContext.Test.FullName.Select(z => Path.GetInvalidFileNameChars().Contains(z) ? '_' : z));
        return Path.Combine(TestContext.CurrentContext.WorkDirectory, "playwright-artefakti", imeTesta);
    }
}
