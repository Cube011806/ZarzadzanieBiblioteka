using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a review in the library management system.
    /// </summary>
    public class Opinia
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the comment for the review.
        /// </summary>
        [MaxLength(1000)]
        public string Komentarz { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating for the review.
        /// </summary>
        [Range(1, 5, ErrorMessage = "Wartość oceny musi być z przedziału od 1 do 5!")]
        public byte Ocena { get; set; }

        /// <summary>
        /// Gets or sets the book ID associated with the review.
        /// </summary>
        public int KsiazkaId { get; set; }

        /// <summary>
        /// Gets or sets the book associated with the review.
        /// </summary>
        public virtual Ksiazka Ksiazka { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user ID who wrote the review.
        /// </summary>
        public string UzytkownikId { get; set; }

        /// <summary>
        /// Gets or sets the user who wrote the review.
        /// </summary>
        public virtual Uzytkownik Uzytkownik { get; set; } = null!;
    }
}
