using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ZarzadzanieBiblioteka.Data;

namespace ZarzadzanieBiblioteka.Controllers
{
    /// <summary>
    /// Base controller class that other controllers inherit from.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        public BaseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Executes before an action method is called.
        /// Retrieves book genres for use in a dropdown menu.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["Gatunki"] = _dbContext.Ksiazki.Select(k => k.Gatunek).Distinct().ToList();

            base.OnActionExecuting(context);
        }
    }
}
