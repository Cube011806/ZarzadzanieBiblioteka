﻿using Microsoft.AspNetCore.Identity;
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
        public IActionResult Add(Ksiazka ksiazka, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
                ksiazka.Okladka = filePath;
            }
            _dbcontext.Add(ksiazka);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
