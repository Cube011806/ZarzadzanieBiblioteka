using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieBiblioteka.Models
{
    /// <summary>
    /// Represents a book in the library management system.
    /// </summary>
    public class Ksiazka
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        [MaxLength(100)]
        public string Tytul { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the genre of the book.
        /// </summary>
        [MaxLength(50)]
        public string Gatunek { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cover image of the book.
        /// </summary>
        public string Okladka { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publication date of the book.
        /// </summary>
        public DateTime DataWydania { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the book.
        /// </summary>
        public int LiczbaStron { get; set; }

        /// <summary>
        /// Gets or sets the type of binding of the book.
        /// </summary>
        [MaxLength(50)]
        public string Oprawa { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the edition of the book.
        /// </summary>
        public int Wydanie { get; set; }

        /// <summary>
        /// Gets or sets the ISBN of the book.
        /// </summary>
        public string ISBN { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the library ID where the book is located.
        /// </summary>
        public int? BibliotekaId { get; set; }

        /// <summary>
        /// Gets or sets the library where the book is located.
        /// </summary>
        public virtual Biblioteka? Biblioteka { get; set; } = null!;

        /// <summary>
        /// Gets or sets the author ID of the book.
        /// </summary>
        public int AutorId { get; set; }

        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        public virtual Autor Autor { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of reviews for the book.
        /// </summary>
        public virtual ICollection<Opinia> Opinie { get; set; } = new List<Opinia>();

        /// <summary>
        /// Gets or sets the collection of volumes for the book.
        /// </summary>
        public virtual ICollection<Wolumin> Woluminy { get; set; } = new List<Wolumin>();
    }
}
