using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Domen;
using Teretana.Api.Infrastruktura.Autentikacija;
using Teretana.Api.Servisi;
using Teretana.Api.Ugovori.Termini;
using Teretana.Api.Ugovori.Zajednicko;

namespace Teretana.Api.Kontroleri;

[ApiController]
[Route("api/termini")]
public sealed class TerminiKontroler(ITerminServis servis) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<StranicaOdgovor<TerminStavkaOdgovor>>> Lista([FromQuery] TerminiUpit upit, CancellationToken cancellationToken) =>
        await servis.VratiStranicuAsync(upit, cancellationToken);

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<TerminDetaljOdgovor>> Detalj(int id, CancellationToken cancellationToken)
    {
        var idClana = User.IsInRole(nameof(Uloga.Clan)) ? User.IdKorisnika() : (int?)null;
        return await servis.VratiDetaljAsync(id, idClana, cancellationToken);
    }

    [HttpPost]
    [Authorize(Policy = Politike.Trener)]
    public async Task<ActionResult<TerminDetaljOdgovor>> Kreiraj(TerminZahtev zahtev, CancellationToken cancellationToken)
    {
        var termin = await servis.KreirajAsync(User.IdKorisnika(), zahtev, cancellationToken);
        return CreatedAtAction(nameof(Detalj), new { id = termin.Id }, termin);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = Politike.Trener)]
    public async Task<ActionResult<TerminDetaljOdgovor>> Izmeni(int id, TerminZahtev zahtev, CancellationToken cancellationToken) =>
        await servis.IzmeniAsync(User.IdKorisnika(), id, zahtev, cancellationToken);

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Politike.Trener)]
    public async Task<IActionResult> Obrisi(int id, CancellationToken cancellationToken)
    {
        await servis.ObrisiAsync(User.IdKorisnika(), id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/otkazivanje")]
    [Authorize(Policy = Politike.Trener)]
    public async Task<ActionResult<TerminDetaljOdgovor>> Otkazi(int id, CancellationToken cancellationToken) =>
        await servis.OtkaziAsync(User.IdKorisnika(), id, cancellationToken);

    [HttpGet("{id:int}/polaznici")]
    [Authorize(Policy = Politike.Trener)]
    public async Task<ActionResult<PolazniciOdgovor>> Polaznici(int id, CancellationToken cancellationToken) =>
        await servis.VratiPolazniceAsync(User.IdKorisnika(), id, cancellationToken);
}
