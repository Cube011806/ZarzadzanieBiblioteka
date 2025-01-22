using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents an author in the library system.
    /// </summary>
    public class Autor
    {
        /// <summary>
        /// Gets or sets the unique identifier for the author.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the author.
        /// </summary>
        [MaxLength(60)]
        public string Imie { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the author.
        /// </summary>
        [MaxLength(60)]
        public string Nazwisko { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of books written by the author.
        /// </summary>
        public virtual ICollection<Ksiazka> Ksiazki { get; set; } = new List<Ksiazka>();
    }
}
