using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Teretana.Api.Infrastruktura.Autentikacija;

public sealed class JwtPodesavanja
{
    public const string Sekcija = "Jwt";

    public string Izdavac { get; set; } = string.Empty;

    public string Publika { get; set; } = string.Empty;

    public string Kljuc { get; set; } = string.Empty;

    public TimeSpan TrajanjeTokena { get; set; }

    public SymmetricSecurityKey KljucZaPotpis() => new(Encoding.UTF8.GetBytes(Kljuc));
}
