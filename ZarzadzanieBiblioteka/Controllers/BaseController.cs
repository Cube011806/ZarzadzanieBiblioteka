using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ZarzadzanieBiblioteka.Data;

namespace ZarzadzanieBiblioteka.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;

        public BaseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Po tym jak inne kontrolery będą po tym głównym dziedziczyć, to będzie za każdym razem wydobywać
        //informacje o gatunkach książek które wykorzystywane są w rozwijanym menu.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["Gatunki"] = _dbContext.Ksiazki.Select(k => k.Gatunek).Distinct().ToList();

            base.OnActionExecuting(context);
        }
    }
}
