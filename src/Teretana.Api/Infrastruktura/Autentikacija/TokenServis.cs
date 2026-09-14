using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Teretana.Api.Domen;

namespace Teretana.Api.Infrastruktura.Autentikacija;

public interface ITokenServis
{
    IzdatToken Izdaj(Korisnik korisnik);
}

public sealed record IzdatToken(string Token, DateTime Istice);

internal sealed class TokenServis(IOptions<JwtPodesavanja> podesavanja, TimeProvider vreme) : ITokenServis
{
    public const string TipClaimaUloge = "role";

    private readonly JsonWebTokenHandler _handler = new();

    public IzdatToken Izdaj(Korisnik korisnik)
    {
        var jwt = podesavanja.Value;
        var sada = vreme.GetUtcNow().UtcDateTime;
        var istice = sada.Add(jwt.TrajanjeTokena);

        var token = _handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = jwt.Izdavac,
            Audience = jwt.Publika,
            IssuedAt = sada,
            NotBefore = sada,
            Expires = istice,
            SigningCredentials = new SigningCredentials(jwt.KljucZaPotpis(), SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = korisnik.Id.ToString(CultureInfo.InvariantCulture),
                [JwtRegisteredClaimNames.Email] = korisnik.Email,
                [JwtRegisteredClaimNames.Name] = korisnik.ImePrezime,
                [TipClaimaUloge] = korisnik.Uloga.ToString(),
            },
        });

        return new IzdatToken(token, istice);
    }
}
