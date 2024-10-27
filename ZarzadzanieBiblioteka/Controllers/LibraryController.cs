using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            //if(ksiazki == null || ksiazki.Count == 0)
            //{
                //TempData["ErrorMessage"] = "Nie udało się pobrać książek z bazy!";
                //return View();
            //}
            return View(ksiazki);
        }
        public IActionResult Add()
        {
            var authors = _dbcontext.Autorzy.Select(a => new
            {
                Id = a.Id,
                Dane = a.Imie + " " + a.Nazwisko
            }).ToList();
            ViewBag.ListaAutorow = new SelectList(authors, "Id", "Dane");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Ksiazka ksiazka, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images/", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                ksiazka.Okladka = filePath;
            }
            else
            {
                TempData["ErrorMessage"] = "Problem z wgraniem okładki książki!";
                return View("Index");
            }
            _dbcontext.Add(ksiazka);
            await _dbcontext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Pomyślnie dodano książkę!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.Find(id);
            return View(ksiazka);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var ksiazkaToDelete = _dbcontext.Ksiazki.FirstOrDefault(ksiazka => ksiazka.Id == id);
            if (ksiazkaToDelete == null)
            {
                TempData["ErrorMessage"] = "Nie udało się pobrać usuwanej książki z bazy!";
                return View("Index");
            }
            _dbcontext.Ksiazki.Remove(ksiazkaToDelete);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie usunięto książkę!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var ksiazkaToEdit = _dbcontext.Ksiazki.FirstOrDefault(ksiazka => ksiazka.Id == id);
            if (ksiazkaToEdit == null)
            {
                TempData["ErrorMessage"] = "Nie udało się pobrać edytowanej książki z bazy!";
                return View("Index");
            }
            return View(ksiazkaToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Ksiazka ksiazka, IFormFile file)
        {
            if (ksiazka.Tytul == null || ksiazka.Gatunek == null || ksiazka.LiczbaStron == 0 || ksiazka.ISBN == null || ksiazka.Wydanie == 0 || ksiazka.Oprawa == null)
            {
                TempData["ErrorMessage"] = "Pola nie mogą być puste!";
                return RedirectToAction("Index");
            }
            var ksiazkaToEdit = _dbcontext.Ksiazki.FirstOrDefault(k => k.Id == ksiazka.Id);
            if(ksiazkaToEdit == null)
            {
                TempData["ErrorMessage"] = "Nie udało się pobrać edytowanej książki z bazy!";
                return RedirectToAction("Index");
            }
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
                ksiazkaToEdit.Okladka = filePath;
            }
            else
            {
                TempData["ErrorMessage"] = "Problem z wgraniem okładki książki!";
                return RedirectToAction("Index");
            }
            ksiazkaToEdit.Tytul = ksiazka.Tytul;
            ksiazkaToEdit.Gatunek = ksiazka.Gatunek;
            ksiazkaToEdit.DataWydania = ksiazka.DataWydania;
            ksiazkaToEdit.LiczbaStron = ksiazka.LiczbaStron;
            ksiazkaToEdit.Oprawa = ksiazka.Oprawa;
            ksiazkaToEdit.Wydanie = ksiazka.Wydanie;
            ksiazkaToEdit.ISBN = ksiazka.ISBN;
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie edytowano książkę!";
            return RedirectToAction("Index");
        }
        public IActionResult IndexAuthors()
        {
            var authors = _dbcontext.Autorzy.ToList();
            return View(authors);
        }
        public IActionResult ViewAuthor(Ksiazka ksiazka)
        {
            var authors = _dbcontext.Autorzy.ToList();
            return View(authors);
        }
        public IActionResult SelectAuthor(Ksiazka ksiazka, int id)
        {
            ksiazka.AutorId = id;
            _dbcontext.Update(ksiazka);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult AddAuthor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAuthor(Autor autor)
        {
            _dbcontext.Autorzy.Add(autor);
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexAuthors");
        }
        public IActionResult EditAuthor(int id)
        {
            var author = _dbcontext.Autorzy.Find(id);
            return View(author);
        }
        [HttpPost]
        public IActionResult EditAuthor(Autor autor)
        {
            _dbcontext.Autorzy.Update(autor);
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexAuthors");
        }

        public IActionResult DeleteAuthor(int id)
        {
            var author = _dbcontext.Autorzy.Find(id);
            if(author != null)
            {
                _dbcontext.Autorzy.Remove(author);
                _dbcontext.SaveChanges();
            }
            return RedirectToAction("IndexAuthors");
        }
    }
}
