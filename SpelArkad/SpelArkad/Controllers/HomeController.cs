using Microsoft.AspNetCore.Mvc;
using SpelArkad.Models;
using System.Diagnostics;
using System.Text.Json;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> Index()
    {
        List<Spel> spelLista = new List<Spel>();

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://informatik7.ei.hv.se/Gameapi/api/Spel");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                spelLista = JsonSerializer.Deserialize<List<Spel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fel vid hämtning av API-data: {ex.Message}");
        }

        return View(spelLista);
    }

    public async Task<IActionResult> UserIndex()
    {
        List<Spel> spelLista = new List<Spel>();

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://informatik7.ei.hv.se/Gameapi/api/Spel");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                spelLista = JsonSerializer.Deserialize<List<Spel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fel vid hämtning av API-data: {ex.Message}");
        }

        return View(spelLista); 
    }

    public IActionResult SkapaSpel()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
