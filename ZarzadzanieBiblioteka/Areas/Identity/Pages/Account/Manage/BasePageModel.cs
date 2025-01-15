using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZarzadzanieBiblioteka.Data;

namespace ZarzadzanieBiblioteka.Areas.Identity.Pages.Account.Manage
{
    public class BasePageModel : PageModel
    {
        protected readonly ApplicationDbContext dbContext;

        public BasePageModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);

            ViewData["Gatunki"] = dbContext.Ksiazki.Select(k => k.Gatunek).Distinct().ToList();
        }
    }
}
