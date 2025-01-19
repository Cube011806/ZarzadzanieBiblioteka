using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZarzadzanieBiblioteka.Data;
using ZarzadzanieBiblioteka.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace ZarzadzanieBiblioteka.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<Uzytkownik> _userManager;
        public LibraryController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index(string SortujPo, string KwerendaWyszukujaca, int? Book1Id, string Gatunek)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            //AsQueryable działa tak że teraz LINQ będzie mógł łączyć kwerendy dynamicznie dzięki czemu 
            //możemy przetworzyć wyszukanie oraz sortowanie jednocześnie i możemy łączyć te dwie funkcje.
            var ksiazki = _dbcontext.Ksiazki.AsQueryable();

            //filtrowanie
            if(!string.IsNullOrEmpty(Gatunek))
            {
                ksiazki = ksiazki.Where(ks => ks.Gatunek == Gatunek);
            }

            //wyszukiwanie
            if (!string.IsNullOrEmpty(KwerendaWyszukujaca))
            {
                ksiazki = ksiazki.Where(ks => ks.Tytul.ToLower().Contains(KwerendaWyszukujaca.ToLower()));
                if (ksiazki.ToList() == null || ksiazki.ToList().Count == 0)
                {
                    TempData["ErrorMessage"] = "Nie udało się znaleźć książki, której tytuł zawierałby taką frazę!";
                    return RedirectToAction("Index");
                }
            }

            //sortowanie
            ksiazki = SortujPo switch
            {
                "Tytul" => ksiazki.OrderBy(ks => ks.Tytul),
                "Autor" => ksiazki.OrderBy(ks => ks.Autor.Imie).ThenBy(ks => ks.Autor.Nazwisko),
                "Gatunek" => ksiazki.OrderBy(ks => ks.Gatunek),
                "Wydanie" => ksiazki.OrderBy(ks => ks.Wydanie),
                "DataWydania" => ksiazki.OrderBy(ks => ks.DataWydania),
                "LiczbaStron" => ksiazki.OrderBy(ks => ks.LiczbaStron),
                _ => ksiazki
            };

            //if(ksiazki == null || ksiazki.Count == 0)
            //{
            //TempData["ErrorMessage"] = "Nie udało się pobrać książek z bazy!";
            //return View();
            //}

            ViewData["SelectedBook1Id"] = Book1Id ?? 0;
            ViewData["Gatuenk"] = Gatunek;
            ViewData["KwerendaWyszukująca"] = KwerendaWyszukujaca;
            ViewData["SortujPo"] = SortujPo;

            return View(ksiazki.ToList());
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
                var placeParh = Path.Combine("images/", file.FileName);
                ksiazka.Okladka = placeParh;
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

            var authors = _dbcontext.Autorzy.Select(a => new
            {
                Id = a.Id,
                Dane = a.Imie + " " + a.Nazwisko
            }).ToList();
            ViewBag.ListaAutorow = new SelectList(authors, "Id", "Dane");
            return View(ksiazkaToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Ksiazka ksiazka, IFormFile file)
        {
            //nulle są sprawdzane na wejściu po stronie klienta ale można zostawić sprawdzanie czy jakaś liczba jest rówa zero
            if (ksiazka.LiczbaStron == 0 || ksiazka.Wydanie == 0 )
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
                    await file.CopyToAsync(stream);
                }
                var placeParh = Path.Combine("images/", file.FileName);
                ksiazkaToEdit.Okladka = placeParh;
            }
            //jeżeli nie ma okładki w editcie, okładka poprzednia jest do niej przypisywana ale nie ma potrzeby na wypisywanie błędu
            ksiazkaToEdit.Tytul = ksiazka.Tytul;
            ksiazkaToEdit.Gatunek = ksiazka.Gatunek;
            ksiazkaToEdit.DataWydania = ksiazka.DataWydania;
            ksiazkaToEdit.LiczbaStron = ksiazka.LiczbaStron;
            ksiazkaToEdit.Oprawa = ksiazka.Oprawa;
            ksiazkaToEdit.Wydanie = ksiazka.Wydanie;
            ksiazkaToEdit.ISBN = ksiazka.ISBN;
            //edycja autora
            ksiazkaToEdit.Autor = ksiazka.Autor;
            ksiazkaToEdit.AutorId = ksiazka.AutorId;
            _dbcontext.Update(ksiazkaToEdit);
            await _dbcontext.SaveChangesAsync();
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
            if (author != null)
            {
                _dbcontext.Autorzy.Remove(author);
                _dbcontext.SaveChanges();
            }
            return RedirectToAction("IndexAuthors");
        }
        public IActionResult AddReview(int idksiazka)
        {
            ViewBag.IdKsiazka = idksiazka;
            return View();
        }
        [HttpPost]
        public IActionResult AddReview(Opinia opinia, int IdKsiazka)
        {
            var uzytkownikId = _userManager.GetUserId(User);

            //sprawdzamy czy użytkownik dodał już opinie o tej książcę
            var czyIstniejeOpinia = _dbcontext.Opinie.FirstOrDefault(op => op.KsiazkaId == IdKsiazka && op.UzytkownikId == uzytkownikId);

            if(czyIstniejeOpinia != null)
            {
                TempData["ErrorMessage"] = "Już dodałeś opinię do tej książki!";
                return RedirectToAction("BookDetails", IdKsiazka);
            }

            opinia.UzytkownikId = uzytkownikId;
            opinia.KsiazkaId = IdKsiazka;
            //var ksiazka = _dbcontext.Ksiazki.Find(IdKsiazka);
            //ksiazka.Opinie.Add(opinia);
            _dbcontext.Opinie.Add(opinia);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie dodano opinię!";
            var ksiazka = _dbcontext.Ksiazki.Include(k => k.Opinie).ThenInclude(o => o.Uzytkownik).FirstOrDefault(k => k.Id == IdKsiazka);
            return View("BookDetails",ksiazka);
        }
        public IActionResult EditReview(int id, int IdKsiazka)
        {
            ViewBag.IdKsiazka = IdKsiazka;
            var opinia = _dbcontext.Opinie.Find(id);
            return View(opinia);
        }
        [HttpPost]
        public IActionResult EditReview(Opinia opinia, int IdKsiazka)
        {
            opinia.UzytkownikId = _userManager.GetUserId(User);
            opinia.KsiazkaId = IdKsiazka;
            //var ksiazka = _dbcontext.Ksiazki.Find(IdKsiazka);
            //ksiazka.Opinie.Add(opinia);
            _dbcontext.Opinie.Update(opinia);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie edytowano opinię!";
            var ksiazka = _dbcontext.Ksiazki.Include(k => k.Opinie).ThenInclude(o => o.Uzytkownik).FirstOrDefault(k => k.Id == IdKsiazka);
            return View("BookDetails", ksiazka);
        }
        public IActionResult DeleteReview(int id)
        {
            var opinia = _dbcontext.Opinie.Find(id);
            var ksiazka = _dbcontext.Ksiazki.Include(k => k.Opinie).ThenInclude(o => o.Uzytkownik).FirstOrDefault(k => k.Id == opinia.KsiazkaId);
            if (opinia != null)
            {
                _dbcontext.Opinie.Remove(opinia);
                _dbcontext.SaveChanges();
                TempData["SuccessMessage"] = "Pomyślnie usunięto opinię!";
                return View("BookDetails", ksiazka);
            }
            TempData["ErrorMessage"] = "Nie udało się usunąć opinii!";
            return View("BookDetails", ksiazka);
        }
        public IActionResult IndexLoans(string KwerendaWyszukujaca)
        {
            var loans = _dbcontext.Wypozyczenia.ToList();
            if(!string.IsNullOrEmpty(KwerendaWyszukujaca))
            {
                loans = _dbcontext.Wypozyczenia.Where(w => w.Uzytkownik.Email.ToLower().Contains(KwerendaWyszukujaca.ToLower())).ToList();
                ViewBag.Rezerwacje = _dbcontext.Rezerwacje.Where(r => r.Uzytkownik.Email.ToLower().Contains(KwerendaWyszukujaca.ToLower())).ToList();
                if((loans == null || loans.Count == 0) && (ViewBag.Rezerwacje == null || ViewBag.Rezerwacje.Count == 0))
                {
                    TempData["ErrorMessage"] = "Nie udało się znaleźć użytkownika, którego email zawierałby taką frazę!";
                    return RedirectToAction("IndexLoans");
                }
            }
            else
            {
                loans = _dbcontext.Wypozyczenia.ToList();
                ViewBag.Rezerwacje = _dbcontext.Rezerwacje.ToList();
            }
            return View(loans);
        }
        public IActionResult IndexVolumes()
        {
            var books = _dbcontext.Ksiazki.ToList();
            var reservations = _dbcontext.Rezerwacje.ToList();

            var booksAndReservations = new Tuple<IEnumerable<Ksiazka>, IEnumerable<Rezerwacja>>(books, reservations);
            return View(booksAndReservations);
        }
        public IActionResult AddVolume(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.FirstOrDefault(k => k.Id == id);
            if (ksiazka == null)
            {
                return NotFound("Nie znaleziono książki o podanym ID.");
            }
            var wolumin = new Wolumin();
            wolumin.KsiazkaId = id;
            ksiazka.Woluminy.Add(wolumin);
            _dbcontext.Woluminy.Add(wolumin);
            _dbcontext.Update(ksiazka);
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexVolumes");
        }
        public IActionResult DeleteVolume(int id)
        {
            var wolumin = _dbcontext.Woluminy.Find(id);
            if (wolumin != null)
            {
                _dbcontext.Woluminy.Remove(wolumin);
            }
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexVolumes");
        }
        public IActionResult CancelReservation (int id)
        {
            var reservation = _dbcontext.Rezerwacje.Find(id);
            if(reservation != null)
            {
                _dbcontext.Rezerwacje.Remove(reservation);
                _dbcontext.SaveChanges();
            }
            return RedirectToAction("IndexLoans");
        }
        public IActionResult LoanVolume(int volid)
        {
            var wolumin = _dbcontext.Woluminy.Find(volid);
            var users = _dbcontext.Uzytkownicy.ToList(); 
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Wolumin = wolumin;
            return View();
        }
        [HttpPost]
        public IActionResult LoanVolume(Wypozyczenie wypozyczenie)
        {
            _dbcontext.Wypozyczenia.Add(wypozyczenie);
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexLoans");
        }
        public IActionResult ReturnVolume(int id)
        {
            var wypozyczenie = _dbcontext.Wypozyczenia.Find(id);
            _dbcontext.Wypozyczenia.Remove(wypozyczenie);
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexLoans");
        }
        public IActionResult AddReservation(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.Find(id);
            if (ksiazka == null)
            {
                return NotFound();
            }

            var wolumin = ksiazka.Woluminy.FirstOrDefault(w => !w.Rezerwacje.Any());
            if (wolumin == null)
            {
                return RedirectToAction("Index"); 
            }

            var userId = _userManager.GetUserId(User);

            var existingReservation = _dbcontext.Rezerwacje
                .FirstOrDefault(r => r.UzytkownikId == userId && r.DataWygasniecia > DateTime.Now);

            if (existingReservation != null)
            {
                TempData["ErrorMessage"] = "Zarezerwowałeś już tę książkę.";
                return RedirectToAction("Index");
            }

            var rezerwacja = new Rezerwacja
            {
                DataRezerwacji = DateTime.Now,
                DataWygasniecia = DateTime.Now.AddDays(14),
                UzytkownikId = userId,
                WoluminId = wolumin.Id
            };

            _dbcontext.Rezerwacje.Add(rezerwacja);
            _dbcontext.SaveChanges();

            TempData["SuccessMessage"] = "Rezerwacja została pomyślnie dodana.";
            return RedirectToAction("Index");
        }
        public IActionResult ExtendLoan(int id)
        {
            var wypozyczenie = _dbcontext.Wypozyczenia.Find(id);
            if (wypozyczenie != null)
            {
                wypozyczenie.DataZwrotu = wypozyczenie.DataZwrotu.AddDays(14);
                _dbcontext.Wypozyczenia.Update(wypozyczenie);
                _dbcontext.SaveChanges();
            }
            return RedirectToAction("IndexLoans");
        }
        //[HttpPost]
        //public IActionResult ConfirmReservation(int id)
        //{
        //    var uzytkownikId = _userManager.GetUserId(User);
        //    var dataRezerwacji = DateTime.Now;
        //    var dataWygasniecia = dataRezerwacji.AddDays(7);
        //    var rezerwcaja = new Rezerwacja();
        //    rezerwcaja.WoluminId = id;
        //    rezerwcaja.UzytkownikId = uzytkownikId;
        //    rezerwcaja.DataRezerwacji = dataRezerwacji;
        //    rezerwcaja.DataWygasniecia = dataWygasniecia;
        //    _dbcontext.Rezerwacje.Add(rezerwcaja);
        //    _dbcontext.SaveChanges();
        //    TempData["SuccessMessage"] = "Pomyślnie zarezerwowano wolumin książki!";
        //    return RedirectToAction("Index");
        //}
        public IActionResult CompareBooks(int book1Id, int book2Id)
        {
            if(book1Id == book2Id)
            {
                TempData["ErrorMessage"] = "Nie możesz porównać tej samej książki ze sobą!";
                return RedirectToAction("Index");
            }

            var book1 = _dbcontext.Ksiazki.Find(book1Id);
            var book2 = _dbcontext.Ksiazki.Find(book2Id);
            if (book1 == null || book2 == null)
            {
                TempData["ErrorMessage"] = "Nie można znaleźć jednej z książek w bazie!";
                return RedirectToAction("Index");
            }

            double avgOcenaBook1 = 0.0;
            double avgOcenaBook2 = 0.0;

            //obliczamy srednia arytmetyczna z wszystkich ocen dla ksiazki1
            if(book1.Opinie.Count > 0)
            {
                avgOcenaBook1 = book1.Opinie.Average(op => op.Ocena);
            }

            //obliczamy srednia arytmetyczna z wszystkich ocen dla ksiazki2
            if (book2.Opinie.Count > 0)
            {
                avgOcenaBook2 = book2.Opinie.Average(op => op.Ocena);
            }

            ViewData["Book1"] = book1;
            ViewData["AvgOcenaBook1"] = avgOcenaBook1;

            ViewData["Book2"] = book2;
            ViewData["AvgOcenaBook2"] = avgOcenaBook2;

            return View();
        }
        public IActionResult IndexManage()
        {
            return View();
        }
        public IActionResult BookDetails(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.Find(id);
            return View(ksiazka);
        }
    }
}
