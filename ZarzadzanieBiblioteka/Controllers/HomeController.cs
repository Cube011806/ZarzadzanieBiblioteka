using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZarzadzanieBiblioteka.Data;
using ZarzadzanieBiblioteka.Models;

namespace ZarzadzanieBiblioteka.Controllers
{
    /// <summary>
    /// Controller responsible for handling home-related actions.
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbcontext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="dbContext">The database context instance.</param>
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The home page view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>The privacy policy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the manage users page.
        /// </summary>
        /// <returns>The manage users view with a list of users.</returns>
        public IActionResult ManageUsers()
        {
            var users = _dbcontext.Uzytkownicy.ToList();
            var adminId = _userManager.GetUserId(User);
            var admin = _dbContext.Uzytkownicy.Find(adminId);
            if(admin != null)
            {
                users.Remove(admin);
            }
            return View(users);
        }

        /// <summary>
        /// Promotes a user to admin.
        /// </summary>
        /// <param name="id">The ID of the user to promote.</param>
        /// <returns>Redirects to the manage users page.</returns>
        public IActionResult MakeAdmin(string id)
        {
            var user = _dbcontext.Uzytkownicy.Find(id);
            user.AccessLevel = 1;
            _dbcontext.Uzytkownicy.Update(user);
            _dbcontext.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        /// <summary>
        /// Demotes an admin to a regular user.
        /// </summary>
        /// <param name="id">The ID of the user to demote.</param>
        /// <returns>Redirects to the manage users page.</returns>
        public IActionResult UnmakeAdmin(string id)
        {
            var user = _dbcontext.Uzytkownicy.Find(id);
            user.AccessLevel = 0;
            _dbcontext.Uzytkownicy.Update(user);
            _dbcontext.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        /// <summary>
        /// Removes a user from the system.
        /// </summary>
        /// <param name="id">The ID of the user to remove.</param>
        /// <returns>Redirects to the manage users page.</returns>
        public IActionResult RemoveUser(string id)
        {
            var user = _dbcontext.Users.Find(id);
            _dbcontext.Users.Remove(user);
            _dbcontext.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        /// <summary>
        /// Displays the error page.
        /// </summary>
        /// <returns>The error view with the error details.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
