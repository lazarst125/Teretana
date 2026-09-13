using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Teretana.KomponentniTestovi.Infrastruktura;

public sealed class TeretanaAplikacija(Action<IServiceCollection> podesiTestneServise) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(podesiTestneServise);
    }
}
