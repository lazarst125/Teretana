using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Infrastruktura.Autentikacija;
using Teretana.Api.Servisi;
using Teretana.Api.Ugovori.Autentikacija;

namespace Teretana.Api.Kontroleri;

[ApiController]
[Route("api/auth")]
public sealed class AutentikacijaKontroler(IAutentikacijaServis servis) : ControllerBase
{
    [HttpPost("registracija")]
    [AllowAnonymous]
    public async Task<ActionResult<KorisnikOdgovor>> Registracija(RegistracijaZahtev zahtev, CancellationToken cancellationToken)
    {
        var korisnik = await servis.RegistrujClanaAsync(zahtev, cancellationToken);
        return CreatedAtAction(nameof(Ja), korisnik);
    }

    [HttpPost("prijava")]
    [AllowAnonymous]
    public async Task<ActionResult<PrijavaOdgovor>> Prijava(PrijavaZahtev zahtev, CancellationToken cancellationToken) =>
        await servis.PrijaviAsync(zahtev, cancellationToken);

    [HttpGet("ja")]
    [Authorize]
    public async Task<ActionResult<KorisnikOdgovor>> Ja(CancellationToken cancellationToken) =>
        await servis.VratiKorisnikaAsync(User.IdKorisnika(), cancellationToken);
}
