using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teretana.Api.Podaci;
using Teretana.Testovi.Zajednicko;

namespace Teretana.KomponentniTestovi.Infrastruktura;

public sealed class TeretanaAplikacija(string okruzenje, Action<IServiceCollection> podesiTestneServise) : WebApplicationFactory<Program>
{
    private readonly IzolovanaBaza _baza = new();

    public string PutanjaBaze => _baza.Putanja;

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        _baza.Dispose();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(okruzenje);
        builder.ConfigureAppConfiguration((_, konfiguracija) => konfiguracija.AddInMemoryCollection(new Dictionary<string, string?>
        {
            [$"ConnectionStrings:{BazaPodatakaRegistracija.NazivConnectionStringa}"] = _baza.ConnectionString,
        }));
        builder.ConfigureTestServices(podesiTestneServise);
    }
}
