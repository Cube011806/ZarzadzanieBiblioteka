using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieBiblioteka.Models
{
	public class Autor
	{
		public int Id { get; set; }

		[MaxLength(60)]
		public string Imie { get; set; } = string.Empty;

		[MaxLength(60)]
		public string Nazwisko { get; set; } = string.Empty;

		public virtual ICollection<Ksiazka> Ksiazki { get; set; } = new List<Ksiazka>();
	}
}
