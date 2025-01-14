using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZarzadzanieBiblioteka.Data;
using ZarzadzanieBiblioteka.Models;

namespace ZarzadzanieBiblioteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbcontext;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ManageUsers()
        {
            var users = _dbcontext.Uzytkownicy.ToList();
            return View(users);
        }
        public IActionResult MakeAdmin(string id)
        {
            var user = _dbcontext.Uzytkownicy.Find(id);
            user.AccessLevel = 1;
            _dbcontext.Uzytkownicy.Update(user);
            _dbcontext.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        public IActionResult UnmakeAdmin(string id)
        {
            var user = _dbcontext.Uzytkownicy.Find(id);
            user.AccessLevel = 0;
            _dbcontext.Uzytkownicy.Update(user);
            _dbcontext.SaveChanges();
            return RedirectToAction("ManageUsers");
        }
        public IActionResult RemoveUser(string id)
        {
            var user = _dbcontext.Users.Find(id);
            _dbcontext.Users.Remove(user);
            _dbcontext.SaveChanges();
            return RedirectToAction("ManageUsers");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
