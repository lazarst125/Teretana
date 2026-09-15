using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Infrastruktura.Autentikacija;
using Teretana.Api.Infrastruktura.Dokumentacija;
using Teretana.Api.Servisi;
using Teretana.Api.Ugovori.Autentikacija;

namespace Teretana.Api.Kontroleri;

/// <summary>Registracija člana, prijava i pregled profila prijavljenog korisnika.</summary>
[ApiController]
[Route("api/auth")]
[Tags("Autentikacija")]
public sealed class AutentikacijaKontroler(IAutentikacijaServis servis) : ControllerBase
{
    /// <summary>Registruje novog člana.</summary>
    /// <remarks>Registracija uvek pravi nalog sa ulogom član; trenerski nalozi postoje samo u demonstracionim podacima.</remarks>
    /// <response code="201">Nalog je kreiran; zaglavlje Location vodi na pregled profila.</response>
    /// <response code="400">Email, ime ili lozinka nisu ispravni (code: validacija).</response>
    /// <response code="409">Nalog sa ovim email-om već postoji (code: email-zauzet).</response>
    [HttpPost("registracija")]
    [AllowAnonymous]
    [ProducesResponseType<KorisnikOdgovor>(StatusCodes.Status201Created)]
    [ValidacioniProblemOdgovor]
    [ProblemOdgovor(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<KorisnikOdgovor>> Registracija(RegistracijaZahtev zahtev, CancellationToken cancellationToken)
    {
        var korisnik = await servis.RegistrujClanaAsync(zahtev, cancellationToken);
        return CreatedAtAction(nameof(Ja), korisnik);
    }

    /// <summary>Prijavljuje korisnika i vraća JWT token.</summary>
    /// <remarks>Token se šalje u zaglavlju <c>Authorization: Bearer &lt;token&gt;</c>. Pogrešna lozinka i nepostojeći email dobijaju isti odgovor.</remarks>
    /// <response code="200">Prijava je uspela.</response>
    /// <response code="400">Email ili lozinka nedostaju (code: validacija).</response>
    /// <response code="401">Email ili lozinka nisu ispravni (code: neispravni-kredencijali).</response>
    [HttpPost("prijava")]
    [AllowAnonymous]
    [ProducesResponseType<PrijavaOdgovor>(StatusCodes.Status200OK)]
    [ValidacioniProblemOdgovor]
    [ProblemOdgovor(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PrijavaOdgovor>> Prijava(PrijavaZahtev zahtev, CancellationToken cancellationToken) =>
        await servis.PrijaviAsync(zahtev, cancellationToken);

    /// <summary>Vraća profil prijavljenog korisnika.</summary>
    /// <response code="200">Profil prijavljenog korisnika.</response>
    [HttpGet("ja")]
    [Authorize]
    [ProducesResponseType<KorisnikOdgovor>(StatusCodes.Status200OK)]
    public async Task<ActionResult<KorisnikOdgovor>> Ja(CancellationToken cancellationToken) =>
        await servis.VratiKorisnikaAsync(User.IdKorisnika(), cancellationToken);
}
