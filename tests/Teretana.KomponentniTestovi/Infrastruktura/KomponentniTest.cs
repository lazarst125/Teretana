using Microsoft.Extensions.DependencyInjection;

namespace Teretana.KomponentniTestovi.Infrastruktura;

/// <summary>
/// Svaki test dobija sopstvenu instancu aplikacije, pa testovi mogu da se izvršavaju paralelno
/// i bilo kojim redosledom.
/// </summary>
[Parallelizable(ParallelScope.All)]
[Category("Komponentni")]
public abstract class KomponentniTest
{
    protected TeretanaAplikacija Aplikacija { get; private set; } = null!;

    protected HttpClient Klijent { get; private set; } = null!;

    [SetUp]
    public void PokreniAplikaciju()
    {
        Aplikacija = new TeretanaAplikacija(PodesiTestneServise);
        Klijent = Aplikacija.CreateClient();
    }

    [TearDown]
    public async Task ZaustaviAplikaciju()
    {
        Klijent.Dispose();
        await Aplikacija.DisposeAsync();
    }

    protected virtual void PodesiTestneServise(IServiceCollection servisi)
    {
    }
}
