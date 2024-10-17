namespace ZarzadzanieBiblioteka.Models
{
	public class Wypozyczenie
	{
		public int Id { get; set; }
		public DateTime DataWypozyczenia { get; set; }
		public DateTime DataZwrotu { get; set; }
		public int WoluminId { get; set; }
		public virtual Wolumin Wolumin { get; set; } = null!;
		public string UzytkownikId { get; set; }
		public virtual Uzytkownik Uzytkownik { get; set; } = null!;
	}
}
