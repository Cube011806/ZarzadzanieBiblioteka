using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a library in the library management system.
    /// </summary>
    public class Biblioteka
    {
        /// <summary>
        /// Gets or sets the unique identifier for the library.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the library.
        /// </summary>
        [MaxLength(50)]
        public string Nazwa { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city where the library is located.
        /// </summary>
        [MaxLength(50)]
        public string Miejscowosc { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the street where the library is located.
        /// </summary>
        [MaxLength(50)]
        public string Ulica { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the building number of the library.
        /// </summary>
        [MaxLength(10)]
        public string NumerBudynku { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal code of the library.
        /// </summary>
        [MaxLength(10)]
        public string KodPocztowy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of books available in the library.
        /// </summary>
        public virtual ICollection<Ksiazka> Ksiazki { get; set; } = new List<Ksiazka>();
    }
}
