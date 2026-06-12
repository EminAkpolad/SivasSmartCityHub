using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartCityHub.Models;
using System.Linq;
using MongoDB.Driver;


namespace SmartCityHub.Controllers;

public class HomeController : Controller
{    
    private readonly MongoDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,MongoDbContext context)
    {
        _logger = logger;
        _context=context;
    }
    public async Task<IActionResult> Index()
    {

        var model=new AnasayfaViewModel();
        model.sonDuyurular=_context.Duyurular
        .Find(d=>true)
        .SortByDescending(d=>d.OlusturmaTarihi)
        .Limit(3)
        .ToList();

        model.AktifIlanlar=_context.Ilanlar
        .Find(d=>d.Aktifmi==true)
        .SortByDescending(d=>d.OlusturmaTarihi)
        .Limit(3)
        .ToList();

        ViewBag.HavaDurumu=await HavaDurumu();

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Harita()
    {
        return View();
    }

    private async Task<string> HavaDurumu()
    {
        try
        {
            using var client=new HttpClient();
            string apiKey="ff8df11a36040eea960d4c4208af3303";
            string url=$"https://api.openweathermap.org/data/2.5/weather?q=Sivas,tr&units=metric&lang=tr&appid={apiKey}";

            var response= await client.GetStringAsync(url);
            using var doc=JsonDocument.Parse(response);
            var sicaklik=doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            return $"{Math.Round(sicaklik)}°C";
        }
        catch
        {
            return "10C";
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
