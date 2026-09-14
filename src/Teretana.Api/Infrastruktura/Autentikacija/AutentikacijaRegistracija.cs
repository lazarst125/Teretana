using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Teretana.Api.Domen;

namespace Teretana.Api.Infrastruktura.Autentikacija;

public static partial class AutentikacijaRegistracija
{
    private const int MinimalnaDuzinaKljucaUBajtovima = 32;

    public static IServiceCollection DodajAutentikaciju(this IServiceCollection servisi)
    {
        servisi.AddOptions<JwtPodesavanja>()
            .BindConfiguration(JwtPodesavanja.Sekcija)
            .PostConfigure<IHostEnvironment>((jwt, okruzenje) =>
            {
                // Aplikacija mora da se pokrene iz čistog klona bez tajni u repozitorijumu, pa se u Development-u
                // ključ generiše pri startu. Posledica: tokeni ne važe posle restarta.
                if (okruzenje.IsDevelopment() && string.IsNullOrEmpty(jwt.Kljuc))
                {
                    jwt.Kljuc = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                }
            })
            .Validate(
                jwt => Encoding.UTF8.GetByteCount(jwt.Kljuc) >= MinimalnaDuzinaKljucaUBajtovima,
                $"Jwt:Kljuc mora da ima najmanje {MinimalnaDuzinaKljucaUBajtovima} bajta. Van Development okruženja postavite ga kroz promenljivu okruženja Jwt__Kljuc.")
            .Validate(
                jwt => !string.IsNullOrWhiteSpace(jwt.Izdavac) && !string.IsNullOrWhiteSpace(jwt.Publika) && jwt.TrajanjeTokena > TimeSpan.Zero,
                "Jwt:Izdavac, Jwt:Publika i Jwt:TrajanjeTokena moraju biti podešeni.")
            .ValidateOnStart();

        servisi.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        servisi.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtPodesavanja>, TimeProvider>((opcije, podesavanja, vreme) =>
            {
                var jwt = podesavanja.Value;
                opcije.MapInboundClaims = false;
                opcije.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwt.Izdavac,
                    ValidAudience = jwt.Publika,
                    IssuerSigningKey = jwt.KljucZaPotpis(),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    NameClaimType = JwtRegisteredClaimNames.Name,
                    RoleClaimType = TokenServis.TipClaimaUloge,
                    // Trajanje tokena se proverava prema istom TimeProvider-u kojim je token izdat,
                    // tako da istek može deterministički da se testira.
                    LifetimeValidator = (vaziOd, vaziDo, _, _) =>
                    {
                        var sada = vreme.GetUtcNow().UtcDateTime;
                        return (vaziOd is null || vaziOd <= sada) && vaziDo is not null && sada < vaziDo;
                    },
                };
            });

        servisi.AddAuthorization();
        servisi.AddSingleton<IPasswordHasher<Korisnik>, PasswordHasher<Korisnik>>();
        servisi.AddSingleton<ITokenServis, TokenServis>();

        return servisi;
    }

    public static void UpozoriAkoJeJwtKljucPrivremen(WebApplication app)
    {
        if (app.Environment.IsDevelopment() && string.IsNullOrEmpty(app.Configuration[$"{JwtPodesavanja.Sekcija}:{nameof(JwtPodesavanja.Kljuc)}"]))
        {
            LogPrivremeniJwtKljuc(app.Logger);
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Jwt:Kljuc nije podešen; koristi se privremeni ključ, pa izdati tokeni ne važe posle restarta aplikacije.")]
    private static partial void LogPrivremeniJwtKljuc(ILogger logger);
}
