using Microsoft.AspNetCore.Identity;

namespace ZarzadzanieBiblioteka.Models
{
    public class Uzytkownik : IdentityUser
    {
        public string Imie { get; set; } = "";
        public string Nazwisko { get; set; } = "";

    }
}
