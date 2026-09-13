using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Teretana.Api.Podaci;

/// <summary>
/// SQLite ne čuva vremensku zonu, a sva vremena u aplikaciji su UTC. Pri upisu se lokalno vreme
/// prevodi u UTC, a pri čitanju se vraća <see cref="DateTimeKind.Utc"/> da se poređenja ne bi pomerila.
/// </summary>
public sealed class UtcDateTimeKonverter() : ValueConverter<DateTime, DateTime>(
    vreme => vreme.Kind == DateTimeKind.Local ? vreme.ToUniversalTime() : DateTime.SpecifyKind(vreme, DateTimeKind.Utc),
    vreme => DateTime.SpecifyKind(vreme, DateTimeKind.Utc));
