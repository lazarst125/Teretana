using Teretana.Api.Repozitorijumi;

namespace Teretana.Api.Servisi;

public static class ServisiRegistracija
{
    public static IServiceCollection DodajAplikacioneServise(this IServiceCollection servisi) =>
        servisi
            .AddScoped<IKorisnikRepozitorijum, KorisnikRepozitorijum>()
            .AddScoped<IAutentikacijaServis, AutentikacijaServis>();
}
