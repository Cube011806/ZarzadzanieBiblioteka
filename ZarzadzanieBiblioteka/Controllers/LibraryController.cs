using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZarzadzanieBiblioteka.Data;
using ZarzadzanieBiblioteka.Models;

namespace ZarzadzanieBiblioteka.Controllers
{
    /// <summary>
    /// Controller for managing the library system.
    /// Provides functionality for books, authors, reviews, reservations, and loans.
    /// </summary>
    public class LibraryController : BaseController
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<Uzytkownik> _userManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager for handling user-related operations.</param>
        /// <param name="dbContext">The application database context.</param>
        public LibraryController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
        }
        /// <summary>
        /// Displays the list of books in the library.
        /// </summary>
        /// <returns>A view with a list of books.</returns>
        public IActionResult Index(string SortujPo, string KwerendaWyszukujaca, int? Book1Id, string Gatunek)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var ksiazki = _dbcontext.Ksiazki.AsQueryable();

            var users = _dbContext.Uzytkownicy.ToList();
            if (users.Count == 1) {
                var admin = users.FirstOrDefault();
                admin.AccessLevel = 1;
                _dbContext.Uzytkownicy.Update(admin);
                _dbContext.SaveChanges();
            }

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
            ViewData["Gatunek"] = Gatunek;
            ViewData["KwerendaWyszukująca"] = KwerendaWyszukujaca;
            ViewData["SortujPo"] = SortujPo;

            return View(ksiazki.ToList());
        }
        /// <summary>
        /// Displays the view with form allowing user to create a book.
        /// </summary>
        /// <returns>A view with a form.</returns>
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
        /// <summary>
        /// Add new book to the system.
        /// </summary>
        /// <param name="ksiazka">Object of class <see cref="Ksiazka"/> created in the form.</param>
        /// <param name="file">File containing cover of the added book.</param>
        /// <returns>A asynchronous task adding book to the database.</returns>
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
        /// <summary>
        /// Displays view with form to confirm deleting book from the database.
        /// </summary>
        /// <param name="id">The id number of the book to be removed.</param>
        /// <returns>Returns view with form to confirm delete.</returns>
        public IActionResult Delete(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.Find(id);
            return View(ksiazka);
        }
        /// <summary>
        /// Removes book from the database.
        /// </summary>
        /// <param name="id">The id number of the book to be removed.</param>
        /// <returns>Returns view with list of all books.</returns>
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
        /// <summary>
        /// Displays form allowing to update book informations.
        /// </summary>
        /// <param name="id">The id number of the book to be edited. It allows to display already filled in form</param>
        /// <returns>Returns view with form to edit a book.</returns>
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
        /// <summary>
        /// Updates book informations.
        /// </summary>
        /// <param name="id">The id number of the book to be edited.</param>
        /// <returns>Returns view with all books.</returns>
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
        /// <summary>
        /// Displays all authors added to the database.
        /// </summary>
        /// <returns>Returns view with list of every author added to the system.</returns>
        public IActionResult IndexAuthors()
        {
            var authors = _dbcontext.Autorzy.ToList();
            return View(authors);
        }

        /// <summary>
        /// Displays view with form allowing adding new author to the database.
        /// </summary>
        /// <returns>Returns the view with form.</returns>
        public IActionResult AddAuthor()
        {
            return View();
        }
        /// <summary>
        /// Adds new author to the database.
        /// </summary>
        /// <param name="autor">Object of class <see cref="Autor"/> filled with information from the form.</param>
        /// <returns>Returns redirection to view IndexAuthors.</returns>
        [HttpPost]
        public IActionResult AddAuthor(Autor autor)
        {
            _dbcontext.Autorzy.Add(autor);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie dodano autora!";
            return RedirectToAction("IndexAuthors");
        }
        /// <summary>
        /// Displays view with form allowing to edit existing author in the database.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Autor"/>.</param>
        /// <returns>Returns the view with form.</returns>
        public IActionResult EditAuthor(int id)
        {
            var author = _dbcontext.Autorzy.Find(id);
            return View(author);
        }
        /// <summary>
        /// Makes changes to existing author in the database.
        /// </summary>
        /// <param name="autor">Object of class <see cref="Autor"/> filled with edited information from the form.</param>
        /// <returns>Returns redirection to view IndexAuthors.</returns>
        [HttpPost]
        public IActionResult EditAuthor(Autor autor)
        {
            _dbcontext.Autorzy.Update(autor);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie edytowano autora!";
            return RedirectToAction("IndexAuthors");
        }
        /// <summary>
        /// Deletes existing author from the database.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Autor"/>.</param>
        /// <returns>Returns redirection to view IndexAuthors.</returns>
        public IActionResult DeleteAuthor(int id)
        {
            var author = _dbcontext.Autorzy.Find(id);
            if (author != null)
            {
                _dbcontext.Autorzy.Remove(author);
                _dbcontext.SaveChanges();
                TempData["SuccessMessage"] = "Pomyślnie usunięto autora!";
            }
            TempData["ErrorMessage"] = "Nie udało się usunąć autora!";
            return RedirectToAction("IndexAuthors");
        }
        /// <summary>
        /// Displays view with form allowing to add new review of the book pointed by idKsiazka.
        /// </summary>
        /// <param name="idKsiazka">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returns the view with form.</returns>
        public IActionResult AddReview(int idKsiazka)
        {
            ViewBag.IdKsiazka = idKsiazka;
            return View();
        }
        /// <summary>
        /// Adds new review created by form to the database and links it with existing book.
        /// </summary>
        /// <param name="opinia">Object of the class <see cref="Opinia" /> created by the form.</param>
        /// <param name="idKsiazka">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returns redirection to view BookDetails.</returns>
        [HttpPost]
        public IActionResult AddReview(Opinia opinia, int IdKsiazka)
        {
            var uzytkownikId = _userManager.GetUserId(User);

            var czyIstniejeOpinia = _dbcontext.Opinie.FirstOrDefault(op => op.KsiazkaId == IdKsiazka && op.UzytkownikId == uzytkownikId);

            if(czyIstniejeOpinia != null)
            {
                TempData["ErrorMessage"] = "Już dodałeś opinię do tej książki!";
                return RedirectToAction("BookDetails", IdKsiazka);
            }

            opinia.UzytkownikId = uzytkownikId;
            opinia.KsiazkaId = IdKsiazka;
            _dbcontext.Opinie.Add(opinia);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie dodano opinię!";
            var ksiazka = _dbcontext.Ksiazki.Include(k => k.Opinie).ThenInclude(o => o.Uzytkownik).FirstOrDefault(k => k.Id == IdKsiazka);
            return View("BookDetails",ksiazka);
        }
        /// <summary>
        /// Displays view with form allowing to edit existing review of the book pointed by idKsiazka.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Opinia"/>.</param>
        /// <param name="idKsiazka">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returns the view with form.</returns>
        public IActionResult EditReview(int id, int IdKsiazka)
        {
            ViewBag.IdKsiazka = IdKsiazka;
            var opinia = _dbcontext.Opinie.Find(id);
            return View(opinia);
        }
        /// <summary>
        /// Edits existing review with data retrieved from form.
        /// </summary>
        /// <param name="opinia">Updated object of the class <see cref="Opinia" /> by data from the form.</param>
        /// <param name="idKsiazka">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returns redirection to view BookDetails.</returns>
        [HttpPost]
        public IActionResult EditReview(Opinia opinia, int IdKsiazka)
        {
            opinia.UzytkownikId = _userManager.GetUserId(User);
            opinia.KsiazkaId = IdKsiazka;
            _dbcontext.Opinie.Update(opinia);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie edytowano opinię!";
            var ksiazka = _dbcontext.Ksiazki.Include(k => k.Opinie).ThenInclude(o => o.Uzytkownik).FirstOrDefault(k => k.Id == IdKsiazka);
            return View("BookDetails", ksiazka);
        }
        /// <summary>
        /// Deletes existing review from the database.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Opinia"/>.</param>
        /// <returns>Returns redirection to view BookDetails.</returns>
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
        /// <summary>
        /// Displays loans and reservations from the database.
        /// </summary>
        /// <param name="KwerendaWyszukujaca">String that allows to filter and sort content of the view.</param>
        /// <returns>Returns view with list of loans and reservations from the database.</returns>
        public IActionResult IndexLoans(string KwerendaWyszukujaca)
        {
            var loans = _dbcontext.Wypozyczenia.ToList();
            if(!string.IsNullOrEmpty(KwerendaWyszukujaca))
            {
                var uzytkownik = _dbContext.Uzytkownicy.FirstOrDefault(u => u.Email.ToLower().Contains(KwerendaWyszukujaca.ToLower()));
                if (uzytkownik == null)
                {
                    TempData["ErrorMessage"] = "Nie udało się znaleźć użytkownika, którego email zawierałby taką frazę!";
                    return RedirectToAction("IndexLoans");
                }
                loans = _dbcontext.Wypozyczenia.Where(w => w.Uzytkownik.Email.ToLower().Contains(KwerendaWyszukujaca.ToLower())).ToList();
                ViewBag.Rezerwacje = _dbcontext.Rezerwacje.Where(r => r.Uzytkownik.Email.ToLower().Contains(KwerendaWyszukujaca.ToLower()) && r.DataWygasniecia > DateTime.Now).ToList();
                if((loans == null || loans.Count == 0) && (ViewBag.Rezerwacje == null || ViewBag.Rezerwacje.Count == 0))
                {
                    TempData["ErrorMessage"] = "Użytkownicy o tej frazie nie mają aktualnie zarezerwowanych lub wypożyczonych książek!";
                    return RedirectToAction("IndexLoans");
                }
            }
            else
            {
                loans = _dbcontext.Wypozyczenia.ToList();
                ViewBag.Rezerwacje = _dbcontext.Rezerwacje.Where(r => r.DataWygasniecia > DateTime.Now).ToList();
            }
            ViewData["KwerendaWyszukujaca"] = KwerendaWyszukujaca;
            return View(loans);
        }
        /// <summary>
        /// Displays list of all volumes for books from the database.
        /// </summary>
        /// <returns>Returns view with list of books and volumes from the database.</returns>
        public IActionResult IndexVolumes()
        {
            var books = _dbcontext.Ksiazki.ToList();
            var reservations = _dbcontext.Rezerwacje.ToList();

            var booksAndReservations = new Tuple<IEnumerable<Ksiazka>, IEnumerable<Rezerwacja>>(books, reservations);
            return View(booksAndReservations);
        }
        /// <summary>
        /// Adds new volume for book from the database.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returns redirection to view IndexVolumes.</returns>
        public IActionResult AddVolume(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.FirstOrDefault(k => k.Id == id);
            if (ksiazka == null)
            {
                return NotFound("Nie znaleziono książki o podanym ID!");
            }
            var wolumin = new Wolumin();
            wolumin.KsiazkaId = id;
            ksiazka.Woluminy.Add(wolumin);
            _dbcontext.Woluminy.Add(wolumin);
            _dbcontext.Update(ksiazka);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie dodano wolumin książki!";
            return RedirectToAction("IndexVolumes");
        }
        /// <summary>
        /// Deletes volume from the database.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Wolumin"/>.</param>
        /// <returns>Returns redirection to view IndexVolumes.</returns>
        public IActionResult DeleteVolume(int id)
        {
            var wolumin = _dbcontext.Woluminy.Find(id);
            if (wolumin != null)
            {
                _dbcontext.Woluminy.Remove(wolumin);
                TempData["SuccessMessage"] = "Pomyślnie usunięto wolumin książki!";
            }
            else
            {
                TempData["ErrorMessage"] = "Nie udało się usunąć woluminu książki!";
            }
            _dbcontext.SaveChanges();
            return RedirectToAction("IndexVolumes");
        }
        /// <summary>
        /// Cancels reservation of the book.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Rezerwacja"/>.</param>
        /// <param name="KwerendaWyszukujaca">String that allows to filter and sort content of the view.</param>
        /// <returns>Returns redirection to view IndexLoans.</returns>
        public IActionResult CancelReservation (int id, string KwerendaWyszukujaca)
        {
            var reservation = _dbcontext.Rezerwacje.Find(id);
            if(reservation != null)
            {
                _dbcontext.Rezerwacje.Remove(reservation);
                _dbcontext.SaveChanges();
                TempData["SuccessMessage"] = "Pomyślnie anulowano rezerwacje!";
            }
            else
            {
                TempData["ErrorMessage"] = "Nie udało się anulować rezerwacji!";
            }
            return RedirectToAction("IndexLoans", new { KwerendaWyszukujaca = KwerendaWyszukujaca });
        }
        /// <summary>
        /// Display form to loan volume to the selected user.
        /// </summary>
        /// <param name="volId">Id number of the existing object of class <see cref="Wolumin"/>.</param>
        /// <returns>Returns redirection to view IndexLoans or returns view with form.</returns>
        public IActionResult LoanVolume(int volId)
        {
            var wolumin = _dbcontext.Woluminy.Find(volId);
            if(wolumin != null)
            {
                var users = new List<Uzytkownik>();
                if (wolumin.Rezerwacje.Count != 0)
                {
                    var userId = wolumin.Rezerwacje.First().Uzytkownik.Id;
                    var user = _dbcontext.Uzytkownicy.Find(userId);
                    var wypozyczenie = new Wypozyczenie
                    {
                        UzytkownikId = userId,
                        WoluminId = wolumin.Id,
                        DataWypozyczenia = DateTime.Now,
                        DataZwrotu = DateTime.Now.AddDays(14)
                    };
                    foreach (var rezerwacja in wolumin.Rezerwacje)
                    {
                        _dbcontext.Remove(rezerwacja);
                    }
                    _dbcontext.Woluminy.Update(wolumin);
                    _dbcontext.Wypozyczenia.Add(wypozyczenie);
                    _dbcontext.SaveChanges();
                    users.Add(user);
                    TempData["SuccessMessage"] = "Pomyślnie wypożyczono wolumin książki!";
                    return RedirectToAction("IndexLoans");
                }
                else
                {
                    users = _dbcontext.Uzytkownicy.ToList();
                }
                ViewBag.Users = new SelectList(users, "Id", "UserName");
                ViewBag.Wolumin = wolumin;
            }
            return View();
        }
        /// <summary>
        /// Loans first available volume to the selected user.
        /// </summary>
        /// <param name="wypozyczenie">Id number of the existing object of class <see cref="Wypozyczenie"/>.</param>
        /// <returns>Returns redirection to view IndexLoans.</returns>
        [HttpPost]
        public IActionResult LoanVolume(Wypozyczenie wypozyczenie)
        {
            var users = _dbcontext.Uzytkownicy.ToList();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            var wolumin = _dbcontext.Woluminy
                .Include(w => w.Ksiazka)
                .FirstOrDefault(w => w.Id == wypozyczenie.WoluminId);
            ViewBag.Wolumin = wolumin;
            wypozyczenie.DataWypozyczenia = DateTime.Now;
            wypozyczenie.DataZwrotu = DateTime.Now.AddDays(14);
            foreach(var rezerwacja in wolumin.Rezerwacje)
            {
                _dbcontext.Remove(rezerwacja);
            }
            _dbcontext.Woluminy.Update(wolumin);
            _dbcontext.Wypozyczenia.Add(wypozyczenie);
            _dbcontext.SaveChanges();

            TempData["SuccessMessage"] = "Pomyślnie wypożyczono wolumin książki!";
            return RedirectToAction("IndexLoans");
        }
        /// <summary>
        /// Allows to return loaned volume.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Wypozyczenie"/>.</param>
        /// <param name="KwerendaWyszukujaca">String that allows to filter and sort content of the view.</param>
        /// <returns>Returns redirection to view IndexLoans.</returns>
        public IActionResult ReturnVolume(int id, string KwerendaWyszukujaca)
        {
            var wypozyczenie = _dbcontext.Wypozyczenia.Find(id);
            _dbcontext.Wypozyczenia.Remove(wypozyczenie);
            _dbcontext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie zwrócono wolumin książki!";
            return RedirectToAction("IndexLoans", new { KwerendaWyszukujaca = KwerendaWyszukujaca });
        }
        /// <summary>
        /// Makes reservation for the first available volume of the book.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returns redirection to view Index, containing list of books.</returns>
        public IActionResult AddReservation(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.Find(id);
            if (ksiazka == null)
            {
                return NotFound();
            }

            var wolumin = ksiazka.Woluminy.FirstOrDefault(w => !w.Rezerwacje.Any() && !w.Wypozyczenia.Any());
            if (wolumin == null)
            {
                return RedirectToAction("Index"); 
            }

            var userId = _userManager.GetUserId(User);

            var existingReservation = _dbcontext.Rezerwacje
                .FirstOrDefault(r => r.UzytkownikId == userId && r.DataWygasniecia > DateTime.Now);

            if (existingReservation != null)
            {
                TempData["ErrorMessage"] = "Masz już zarezerwowaną jedną książkę!";
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

            TempData["SuccessMessage"] = "Pomyślnie zarezerwowano książkę!";
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Extends the loan period of selected volume by 2 weeks.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Wypozyczenie"/>.</param>
        /// <param name="KwerendaWyszukujaca">String that allows to filter and sort content of the view.</param>
        /// <returns>Returns redirection to view IndexLoans, containing list of loans and reservations.</returns>
        public IActionResult ExtendLoan(int id, string KwerendaWyszukujaca)
        {
            var wypozyczenie = _dbcontext.Wypozyczenia.Find(id);
            if (wypozyczenie != null)
            {
                wypozyczenie.DataZwrotu = wypozyczenie.DataZwrotu.AddDays(14);
                _dbcontext.Wypozyczenia.Update(wypozyczenie);
                TempData["SuccessMessage"] = "Pomyślnie przedłużono wypożyczenie książki!";
                _dbcontext.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Nie udało się przedłużyć wypożyczenia książki!";
            }
            return RedirectToAction("IndexLoans", new { KwerendaWyszukujaca = KwerendaWyszukujaca });
        }
        /// <summary>
        /// Compares selected books.
        /// </summary>
        /// <param name="book1Id">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <param name="book2Id">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returs redirection to view which contains comparison between selected books.</returns>
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

            if(book1.Opinie.Count > 0)
            {
                avgOcenaBook1 = book1.Opinie.Average(op => op.Ocena);
            }

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
        /// <summary>
        /// Displays the list of library management exclusive functions.
        /// </summary>
        /// <returns>Returns view with options designed for use by library employees.</returns>
        public IActionResult IndexManage()
        {
            return View();
        }
        /// <summary>
        /// Displays view with details about selected book.
        /// </summary>
        /// <param name="id">Id number of the existing object of class <see cref="Ksiazka"/>.</param>
        /// <returns>Returs view which contains all available data about selected book.</returns>
        public IActionResult BookDetails(int id)
        {
            var ksiazka = _dbcontext.Ksiazki.Find(id);
            return View(ksiazka);
        }
    }
}
