namespace ZarzadzanieBiblioteka.Models
{
	public class Wypozyczenie
	{
		public int Id { get; set; }
		public DateTime DataWypozyczenia { get; set; }
		public DateTime DataZwrotu { get; set; }
		public int KsiazkaId { get; set; }
		public virtual Ksiazka Ksiazka { get; set; } = null!;
		public int UzytkownikId { get; set; }
		public virtual Uzytkownik Uzytkownik { get; set; } = null!;
	}
}
