using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Services;

namespace SmartCityHub.Controllers
{
    public class IlanlarController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public IlanlarController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            var ilanlar = await _mongoDbService.Ilanlar.Find(_ => true).ToListAsync();
            return View(ilanlar);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Ilan YeniIlan)
        {
            if (ModelState.IsValid)
            {
                YeniIlan.Tarih = DateTime.Now;
                YeniIlan.Aktifmi = true;

                await _mongoDbService.Ilanlar.InsertOneAsync(YeniIlan);

                return RedirectToAction("Index");
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine("MODEL HATASI: " + error.ErrorMessage);
                }
            }

            return View(YeniIlan);
        }

        public async Task<IActionResult> Detay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var ilan = await _mongoDbService.Ilanlar.Find(i => i.Id == id).FirstOrDefaultAsync();

            if (ilan == null)
            {
                return NotFound();
            }
            return View(ilan);
        }
    }
}
