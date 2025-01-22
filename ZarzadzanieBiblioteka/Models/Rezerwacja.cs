namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a reservation in the library management system.
    /// </summary>
    public class Rezerwacja
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date of the reservation.
        /// </summary>
        public DateTime DataRezerwacji { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the reservation.
        /// </summary>
        public DateTime DataWygasniecia { get; set; }

        /// <summary>
        /// Gets or sets the volume ID associated with the reservation.
        /// </summary>
        public int WoluminId { get; set; }

        /// <summary>
        /// Gets or sets the volume associated with the reservation.
        /// </summary>
        public virtual Wolumin Wolumin { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user ID who made the reservation.
        /// </summary>
        public string UzytkownikId { get; set; }

        /// <summary>
        /// Gets or sets the user who made the reservation.
        /// </summary>
        public virtual Uzytkownik Uzytkownik { get; set; } = null!;
    }
}
