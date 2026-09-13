namespace Teretana.Testovi.Zajednicko;

/// <summary>
/// Privremeni SQLite fajl koji pripada jednom testu (ili jednoj klasi testova) i briše se posle njega.
/// Bez pooling-a se konekcija zatvara odmah, pa fajl može da se obriše čim test završi.
/// </summary>
public sealed class IzolovanaBaza : IDisposable
{
    private static readonly string Folder = Path.Combine(Path.GetTempPath(), "teretana-testovi");

    public IzolovanaBaza()
    {
        Directory.CreateDirectory(Folder);
        Putanja = Path.Combine(Folder, $"{Guid.NewGuid():N}.db");
    }

    public string Putanja { get; }

    public string ConnectionString => $"Data Source={Putanja};Pooling=False";

    public void Dispose()
    {
        foreach (var fajl in new[] { Putanja, $"{Putanja}-wal", $"{Putanja}-shm", $"{Putanja}-journal" })
        {
            File.Delete(fajl);
        }
    }
}
