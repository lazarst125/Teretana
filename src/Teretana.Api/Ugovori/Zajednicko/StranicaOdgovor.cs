namespace Teretana.Api.Ugovori.Zajednicko;

public sealed record StranicaOdgovor<T>(IReadOnlyList<T> Stavke, int Stranica, int VelicinaStranice, int UkupnoStavki)
{
    public int UkupnoStranica => (int)Math.Ceiling(UkupnoStavki / (double)VelicinaStranice);
}
