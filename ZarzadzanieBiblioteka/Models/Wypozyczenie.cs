namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a borrowing in the library management system.
    /// </summary>
    public class Wypozyczenie
    {
        /// <summary>
        /// Gets or sets the unique identifier for the borrowing.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the borrowing date.
        /// </summary>
        public DateTime DataWypozyczenia { get; set; }

        /// <summary>
        /// Gets or sets the return date.
        /// </summary>
        public DateTime DataZwrotu { get; set; }

        /// <summary>
        /// Gets or sets the volume ID associated with the borrowing.
        /// </summary>
        public int WoluminId { get; set; }

        /// <summary>
        /// Gets or sets the volume associated with the borrowing.
        /// </summary>
        public virtual Wolumin Wolumin { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user ID who borrowed the volume.
        /// </summary>
        public string UzytkownikId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user who borrowed the volume.
        /// </summary>
        public virtual Uzytkownik Uzytkownik { get; set; } = null!;
    }
}
