using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieBiblioteka.Models
{
	public class Ksiazka
	{
		public int Id { get; set; }

		[MaxLength(100)]
		public string Tytul { get; set; } = string.Empty;

		[MaxLength(50)]
		public string Gatunek { get; set; } = string.Empty;
        public string Okladka { get; set; } = string.Empty;
        public DateTime DataWydania { get; set; }
		public int LiczbaStron { get; set; }
		[MaxLength(50)]
		public string Oprawa { get; set; } = string.Empty;
		public int Wydanie { get; set; }
		public string ISBN { get; set; } = string.Empty;
		//Relacje
		public int? BibliotekaId { get; set; }							//
		public virtual Biblioteka? Biblioteka { get; set; } = null!;	//Obie te relacje są tymczasowo jako nullable, by baza się nie buntowała przy próbie wpisania ksiazki
		public int AutorId { get; set; }
		public virtual Autor Autor { get; set; } = null!;
		public virtual ICollection<Opinia> Opinie { get; set; } = new List<Opinia>();
        public virtual ICollection<Wolumin> Woluminy { get; set; } = new List<Wolumin>();
    }
}
