namespace ZarzadzanieBiblioteka.Models
{
	public class Rezerwacja
	{
		public int Id { get; set; }
		public DateTime DataRezerwacji { get; set; }
		public DateTime DataWygasniecia { get; set; }
		//Relacje
		public int WoluminId { get; set; }
		public virtual Wolumin Wolumin { get; set; } = null!;
		public string UzytkownikId { get; set; }
		public virtual Uzytkownik Uzytkownik { get; set; } = null!;
	}
}
