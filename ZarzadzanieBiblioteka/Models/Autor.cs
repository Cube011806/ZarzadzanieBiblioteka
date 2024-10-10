using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieBiblioteka.Models
{
	public class Autor
	{
		public int Id { get; set; }

		[Column(TypeName = "varchar2(60)")]
		[MaxLength(60)]
		public string ImieAutora { get; set; } = string.Empty;

		[Column(TypeName = "varchar2(60)")]
		[MaxLength(60)]
		public string NazwiskoAutora { get; set; } = string.Empty;

		public virtual ICollection<Ksiazka> Ksiazki { get; set; } = new List<Ksiazka>();
	}
}
