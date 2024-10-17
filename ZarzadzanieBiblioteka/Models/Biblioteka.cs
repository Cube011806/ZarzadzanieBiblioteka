using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZarzadzanieBiblioteka.Models
{
	public class Biblioteka
	{
		public int Id { get; set; }

		//[Column(TypeName = "varchar2(50)")]
		[MaxLength(50)]
		public string Nazwa { get; set; } = string.Empty;

		//[Column(TypeName = "varchar2(50)")]
		[MaxLength(50)]
		public string Miejscowosc { get; set; } = string.Empty;

		//[Column(TypeName = "varchar2(50)")]
		[MaxLength(50)]
		public string Ulica { get; set; } = string.Empty;

		//[Column(TypeName = "varchar2(10)")]
		[MaxLength(10)]
		public string NumerBudynku { get; set; } = string.Empty;

		//[Column(TypeName = "varchar2(10)")]
		[MaxLength(10)]
		public string KodPocztowy { get; set; } = string.Empty;

		public virtual ICollection<Ksiazka> Ksiazki { get; set; } = new List<Ksiazka>();
	}
}
