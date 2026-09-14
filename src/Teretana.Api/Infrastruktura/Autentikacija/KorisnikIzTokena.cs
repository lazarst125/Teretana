using System.Globalization;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Teretana.Api.Infrastruktura.Autentikacija;

internal static class KorisnikIzTokena
{
    public static int IdKorisnika(this ClaimsPrincipal korisnik)
    {
        var sub = korisnik.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new InvalidOperationException("Autentifikovani korisnik nema 'sub' claim.");
        return int.Parse(sub, CultureInfo.InvariantCulture);
    }
}
