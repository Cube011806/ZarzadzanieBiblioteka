namespace ZarzadzanieBiblioteka.Models
{
	public class Rezerwacja
	{
		public int Id { get; set; }
		public DateTime DataRezerwacji { get; set; }
		public DateTime DataWygasniecia { get; set; }
		public int KsiazkaId { get; set; }
		public virtual Ksiazka Ksiazka { get; set; } = null!;
		public int UzytkownikId { get; set; }
		public virtual Uzytkownik Uzytkownik { get; set; } = null!;
	}
}
