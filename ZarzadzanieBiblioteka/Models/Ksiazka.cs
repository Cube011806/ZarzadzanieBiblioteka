using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieBiblioteka.Models
{
	public class Ksiazka
	{
		public int Id { get; set; }

		//[Column(TypeName = "varchar2(100)")]
		[MaxLength(100)]
		public string Tytul { get; set; } = string.Empty;

		//[Column(TypeName = "varchar2(30)")]
		[MaxLength(30)]
		public string Gatunek { get; set; } = string.Empty;
		public DateTime DataWydania { get; set; }
		public int LiczbaStron { get; set; }
		public string Oprawa { get; set; } = string.Empty;
		public int Wydanie { get; set; }
		public int ISBN { get; set; }
		//Relacje
		public int BibliotekaId { get; set; }
		public virtual Biblioteka Biblioteka { get; set; } = null!;
		public int AutorId { get; set; }
		public virtual Autor Autor { get; set; } = null!;
		public virtual ICollection<Opinia> Opinie { get; set; } = new List<Opinia>();
		public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
		public virtual ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
	}
}
