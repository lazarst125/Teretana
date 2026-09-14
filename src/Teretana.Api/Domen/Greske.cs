namespace Teretana.Api.Domen;

public static class Greske
{
    public static DomenskaGreska EmailZauzet() =>
        new(VrstaGreske.Konflikt, "email-zauzet", "Nalog sa ovim email-om već postoji.");

    public static DomenskaGreska NeispravniKredencijali() =>
        new(VrstaGreske.NijeAutorizovan, "neispravni-kredencijali", "Email ili lozinka nisu ispravni.");

    public static DomenskaGreska KorisnikIzTokenaNePostoji() =>
        new(VrstaGreske.NijeAutorizovan, "korisnik-ne-postoji", "Korisnik iz tokena više ne postoji.");
}
