namespace Teretana.Api.Domen;

public static class Greske
{
    public static DomenskaGreska EmailZauzet() =>
        new(VrstaGreske.Konflikt, "email-zauzet", "Nalog sa ovim email-om već postoji.");

    public static DomenskaGreska NeispravniKredencijali() =>
        new(VrstaGreske.NijeAutorizovan, "neispravni-kredencijali", "Email ili lozinka nisu ispravni.");

    public static DomenskaGreska KorisnikIzTokenaNePostoji() =>
        new(VrstaGreske.NijeAutorizovan, "korisnik-ne-postoji", "Korisnik iz tokena više ne postoji.");

    public static DomenskaGreska TerminNePostoji() =>
        new(VrstaGreske.NijePronadjeno, "termin-ne-postoji", "Termin ne postoji.");

    public static DomenskaGreska TudjiTermin() =>
        new(VrstaGreske.Zabranjeno, "tudji-termin", "Trener može da upravlja samo svojim terminima.");

    public static DomenskaGreska TerminOtkazan() =>
        new(VrstaGreske.Konflikt, "termin-otkazan", "Termin je otkazan.");

    public static DomenskaGreska TerminImaPrijave() =>
        new(VrstaGreske.Konflikt, "termin-ima-prijave", "Termin ima prijave i ne može da se menja ni briše; umesto toga ga otkažite.");

    public static DomenskaGreska TerminJePoceo() =>
        new(VrstaGreske.PoslovnoPravilo, "termin-je-poceo", "Termin je već počeo.");

    public static DomenskaGreska PocetakUProslosti() =>
        new(VrstaGreske.PoslovnoPravilo, "pocetak-u-proslosti", "Početak termina mora biti u budućnosti.");
}
