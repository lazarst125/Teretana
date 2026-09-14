using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teretana.Api.Infrastruktura.Autentikacija;
using Teretana.Api.Servisi;
using Teretana.Api.Ugovori.Rezervacije;
using Teretana.Api.Ugovori.Zajednicko;

namespace Teretana.Api.Kontroleri;

[ApiController]
[Route("api")]
public sealed class RezervacijeKontroler(IRezervacijaServis servis) : ControllerBase
{
    [HttpPost("termini/{idTermina:int}/rezervacije")]
    [Authorize(Policy = Politike.Clan)]
    public async Task<ActionResult<RezervacijaOdgovor>> Rezervisi(int idTermina, CancellationToken cancellationToken)
    {
        var rezervacija = await servis.RezervisiAsync(User.IdKorisnika(), idTermina, cancellationToken);
        return CreatedAtAction(nameof(Detalj), new { id = rezervacija.Id }, rezervacija);
    }

    [HttpPost("termini/{idTermina:int}/lista-cekanja")]
    [Authorize(Policy = Politike.Clan)]
    public async Task<ActionResult<RezervacijaOdgovor>> PrijaviNaListuCekanja(int idTermina, CancellationToken cancellationToken)
    {
        var prijava = await servis.PrijaviNaListuCekanjaAsync(User.IdKorisnika(), idTermina, cancellationToken);
        return CreatedAtAction(nameof(Detalj), new { id = prijava.Id }, prijava);
    }

    [HttpGet("rezervacije/moje")]
    [Authorize(Policy = Politike.Clan)]
    public async Task<ActionResult<StranicaOdgovor<RezervacijaOdgovor>>> Moje([FromQuery] MojeRezervacijeUpit upit, CancellationToken cancellationToken) =>
        await servis.VratiMojeAsync(User.IdKorisnika(), upit, cancellationToken);

    [HttpGet("rezervacije/{id:int}")]
    [Authorize(Policy = Politike.Clan)]
    public async Task<ActionResult<RezervacijaOdgovor>> Detalj(int id, CancellationToken cancellationToken) =>
        await servis.VratiAsync(User.IdKorisnika(), id, cancellationToken);

    [HttpPut("rezervacije/{id:int}/prisustvo")]
    [Authorize(Policy = Politike.Trener)]
    public async Task<ActionResult<RezervacijaOdgovor>> EvidentirajPrisustvo(int id, PrisustvoZahtev zahtev, CancellationToken cancellationToken) =>
        await servis.EvidentirajPrisustvoAsync(User.IdKorisnika(), id, zahtev.Prisustvovao!.Value, cancellationToken);

    [HttpDelete("rezervacije/{id:int}")]
    [Authorize(Policy = Politike.Clan)]
    public async Task<IActionResult> Otkazi(int id, CancellationToken cancellationToken)
    {
        await servis.OtkaziAsync(User.IdKorisnika(), id, cancellationToken);
        return NoContent();
    }
}
