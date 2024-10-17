using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZarzadzanieBiblioteka.Data;
using ZarzadzanieBiblioteka.Models;

namespace ZarzadzanieBiblioteka.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<Uzytkownik> _userManager;
        public LibraryController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var ksiazki = _dbcontext.Ksiazki.ToList();
            return View(ksiazki);
        }
        public IActionResult Add()
        {
            return View();
        }
    }
}
