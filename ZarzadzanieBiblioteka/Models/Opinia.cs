﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZarzadzanieBiblioteka.Models
{
	public class Opinia
	{
		public int Id { get; set; }

		[MaxLength(1000)]
		public string Komentarz { get; set; } = string.Empty;

		[Range(1, 5, ErrorMessage = "Wartość oceny musi być z przedziału od 1 do 5!")]
		public byte Ocena { get; set; }
		public int KsiazkaId { get; set; }
		public virtual Ksiazka Ksiazka { get; set; } = null!;
		public string UzytkownikId { get; set; }
		public virtual Uzytkownik Uzytkownik { get; set; } = null!;
	}
}
