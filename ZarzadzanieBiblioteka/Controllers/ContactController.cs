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
    /// <summary>
    /// Returns view Index.cshtml.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
    /// <summary>
    /// The action responsible for calling the SendEmailAsync method, which sends an email with the content entered by the user.
    /// </summary>
    /// <param name="subject">Contains the subject of the message</param>
    /// <param name="message">Contains the content of the message</param>
    /// <returns>Returns redirection to Index.cshtml</returns>
    [HttpPost]
    public async Task<IActionResult> Send(string subject, string message)
    {
        var adminEmail = "example@gmail.com"; 
        await _emailService.SendEmailAsync(adminEmail, subject, message);

        TempData["SuccessMessage"] = "Twoja wiadomość została wysłana.";
        return RedirectToAction("Index");
    }
}
