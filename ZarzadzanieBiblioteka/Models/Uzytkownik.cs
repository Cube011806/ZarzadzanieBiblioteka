using Microsoft.AspNetCore.Identity;

namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a user in the library management system.
    /// </summary>
    public class Uzytkownik : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string Imie { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string Nazwisko { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the access level of the user.
        /// </summary>
        public int AccessLevel { get; set; } = 0; //Do zarządzania uprawnieniami

        /// <summary>
        /// Gets or sets the birth date of the user.
        /// </summary>
        public DateTime DataUrodzenia { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews written by the user.
        /// </summary>
        public virtual ICollection<Opinia> Opinie { get; set; } = new List<Opinia>();

        /// <summary>
        /// Gets or sets the collection of borrowings made by the user.
        /// </summary>
        public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();

        /// <summary>
        /// Gets or sets the collection of reservations made by the user.
        /// </summary>
        public virtual ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
