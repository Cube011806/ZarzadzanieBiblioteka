using Microsoft.AspNetCore.Identity;

namespace ZarzadzanieBiblioteka.Models
{
    public class Uzytkownik : IdentityUser
    {
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public int AccessLevel { get; set; } = 0; //Do zarządzania uprawnieniami
        public DateTime DataUrodzenia { get; set; }

        /*
         *   Pamiętać: Id w Uzytkownik zawsze jest typu string!!!
         *   Reszta pól jest już gwarantowana przez Identity
        */

        //Relacje
        public virtual ICollection<Opinia> Opinie { get; set; } = new List<Opinia>();
        public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
        public virtual ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
