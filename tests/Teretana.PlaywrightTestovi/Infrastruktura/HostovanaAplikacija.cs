using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Teretana.PlaywrightTestovi.Infrastruktura;

/// <summary>
/// Aplikacija na pravom Kestrel serveru i slobodnom portu, da bi browser i APIRequestContext
/// razgovarali sa njom preko HTTP-a, bez ručnog pokretanja servera.
/// </summary>
public sealed class HostovanaAplikacija : WebApplicationFactory<Program>
{
    public HostovanaAplikacija() => UseKestrel(opcije => opcije.Listen(IPAddress.Loopback, 0));

    public string Adresa { get; private set; } = string.Empty;

    public void Pokreni()
    {
        StartServer();
        Adresa = Services.GetRequiredService<IServer>()
            .Features.GetRequiredFeature<IServerAddressesFeature>()
            .Addresses.Single();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseEnvironment("Testing");
}
