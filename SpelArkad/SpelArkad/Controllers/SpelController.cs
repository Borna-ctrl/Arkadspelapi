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

    public IActionResult SkapaSpel()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SkapaSpel(Spel spel)
    {
        if (ModelState.IsValid)
        {
            await _spelService.CreateSpelAsync(spel);
            return RedirectToAction("Index", "Home");
        }
        return View(spel);
    }

    [HttpPost]
    public async Task<IActionResult> RaderaSpel(int id)
    {
        try
        {
            await _spelService.RaderaSpelAsync(id);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Fel vid radering av spel: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
