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

        [HttpPost]
        public async Task<IActionResult> Add(Ksiazka ksiazka, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                ksiazka.Okladka = filePath;
            }
            _dbcontext.Add(ksiazka);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var ksiazkaToDelete = _dbcontext.Ksiazki.FirstOrDefault(ksiazka => ksiazka.Id == id);
            if(ksiazkaToDelete == null)
            {
                return View("Index");
            }
            _dbcontext.Ksiazki.Remove(ksiazkaToDelete);
            _dbcontext.SaveChanges();
            return View("Index");
        }
    }
}
