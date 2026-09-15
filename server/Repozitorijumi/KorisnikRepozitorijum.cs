using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Teretana.Api.Domen;
using Teretana.Api.Podaci;

namespace Teretana.Api.Repozitorijumi;

public interface IKorisnikRepozitorijum
{
    Task<bool> PostojiEmailAsync(string email, CancellationToken cancellationToken);

    Task<Korisnik?> PronadjiPoEmailuAsync(string email, CancellationToken cancellationToken);

    Task<Korisnik?> PronadjiPoIdAsync(int id, CancellationToken cancellationToken);

    Task DodajAsync(Korisnik korisnik, CancellationToken cancellationToken);
}

internal sealed class KorisnikRepozitorijum(TeretanaDbContext db) : IKorisnikRepozitorijum
{
    private const int SqliteConstraintUnique = 2067;

    public Task<bool> PostojiEmailAsync(string email, CancellationToken cancellationToken) =>
        db.Korisnici.AnyAsync(k => k.Email == email, cancellationToken);

    public Task<Korisnik?> PronadjiPoEmailuAsync(string email, CancellationToken cancellationToken) =>
        db.Korisnici.SingleOrDefaultAsync(k => k.Email == email, cancellationToken);

    public Task<Korisnik?> PronadjiPoIdAsync(int id, CancellationToken cancellationToken) =>
        db.Korisnici.AsNoTracking().SingleOrDefaultAsync(k => k.Id == id, cancellationToken);

    public async Task DodajAsync(Korisnik korisnik, CancellationToken cancellationToken)
    {
        db.Korisnici.Add(korisnik);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException greska) when (greska.InnerException is SqliteException { SqliteExtendedErrorCode: SqliteConstraintUnique })
        {
            // Dve istovremene registracije istog email-a prođu proveru u servisu; jedinstveni indeks odlučuje.
            throw Greske.EmailZauzet();
        }
    }
}
