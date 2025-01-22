namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a volume in the library management system.
    /// </summary>
    public class Wolumin
    {
        /// <summary>
        /// Gets or sets the unique identifier for the volume.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the book associated with the volume.
        /// </summary>
        public virtual Ksiazka Ksiazka { get; set; }

        /// <summary>
        /// Gets or sets the book ID associated with the volume.
        /// </summary>
        public int KsiazkaId { get; set; }

        /// <summary>
        /// Gets or sets the collection of borrowings for the volume.
        /// </summary>
        public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();

        /// <summary>
        /// Gets or sets the collection of reservations for the volume.
        /// </summary>
        public virtual ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
