﻿namespace ZarzadzanieBiblioteka.Models
{
    public class Wolumin
    {
        public int Id { get; set; }

        //Relacje
        public virtual Ksiazka Ksiazka { get; set; }
        public int KsiazkaId { get; set; }
        public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
        public virtual ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
