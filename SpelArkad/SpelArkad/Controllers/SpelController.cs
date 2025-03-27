using Microsoft.AspNetCore.Mvc;
using SpelArkad.Models;
using SpelArkad.Services;

public class SpelController : Controller
{
    private readonly SpelService _spelService;

    public SpelController(SpelService spelService)
    {
        _spelService = spelService;
    }

    public async Task<IActionResult> Index()
    {
        var spelLista = await _spelService.GetSpelAsync();
        return View(spelLista);
    }

    // Visa formuläret för att skapa ett nytt spel
    public IActionResult SkapaSpel()
    {
        return View();
    }

    // Skapa ett nytt spel
    [HttpPost]
    public async Task<IActionResult> SkapaSpel(Spel spel)
    {
        if (ModelState.IsValid)
        {
            await _spelService.CreateSpelAsync(spel);
            return RedirectToAction("Index", "Home"); // Gå tillbaka till listan
        }
        return View(spel);
    }

    // Radera ett spel baserat på ID
    [HttpPost]
    public async Task<IActionResult> RaderaSpel(int id)
    {
        try
        {
            // Anropa SpelService för att radera spelet från databasen
            await _spelService.RaderaSpelAsync(id);
            return RedirectToAction("Index", "Home"); // Gå tillbaka till spelsidan efter radering
        }
        catch (Exception ex)
        {
            // Hantera fel vid radering (logga eller visa meddelande)
            ViewBag.ErrorMessage = $"Fel vid radering av spel: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
