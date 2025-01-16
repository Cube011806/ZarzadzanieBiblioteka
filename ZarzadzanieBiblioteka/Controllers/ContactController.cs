using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZarzadzanieBiblioteka.Controllers;
using ZarzadzanieBiblioteka.Data;
using ZarzadzanieBiblioteka.Models;

public class ContactController : BaseController
{
    private readonly EmailService _emailService;

    public ContactController(ApplicationDbContext dbContext, EmailService emailService) : base(dbContext)
    {
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Send(string subject, string message)
    {
        var adminEmail = "admin@example.com"; // Adres e-mail administratora
        await _emailService.SendEmailAsync(adminEmail, subject, message);

        TempData["SuccessMessage"] = "Twoja wiadomość została wysłana.";
        return RedirectToAction("Index");
    }
}
