using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SmartCityHub.Controllers
{
    public class IlanlarController : Controller
    {
        private readonly IlanService _ilanService;

        public IlanlarController(IlanService ilanService)
        {
            _ilanService = ilanService;
        }

        public async Task<IActionResult> Index()
        {
            var ilanlar = await _ilanService.HepsiniGetir();
            return View(ilanlar);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Ilan YeniIlan, IFormFile? resimDosyasi)
        {
            var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                if (resimDosyasi != null && resimDosyasi.Length > 0)
                {
                    var dosyaAdi =
                        Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);

                    var yol = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        dosyaAdi
                    );
                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(stream);
                    }

                    YeniIlan.ResimUrl = "/uploads/" + dosyaAdi;
                }
                YeniIlan.Tarih = DateTime.Now;
                YeniIlan.Aktifmi = true;

                await _ilanService.Ekle(YeniIlan, kullaniciId);
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

        [HttpGet]
        public async Task<IActionResult> Guncelle(string id)
        {
            var ilan = await _ilanService.GetirById(id);
            if (ilan == null)
                return NotFound();
            return View(ilan);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(string id, Ilan GuncelIlan, IFormFile? Resim)
        {
            var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                if (Resim != null && Resim.Length > 0)
                {
                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(Resim.FileName);
                    var yol = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    dosyaAdi
                    );

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        await Resim.CopyToAsync(stream);
                    }
                    GuncelIlan.ResimUrl = "/uploads/" + dosyaAdi;
                }
                await _ilanService.Guncelle(id, GuncelIlan);
                return RedirectToAction(nameof(Index));
            }
            return View(GuncelIlan);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(string id)
        {
            var ilan = await _ilanService.GetirById(id);
            var girisYapanId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (ilan != null && (ilan.KullaniciId == girisYapanId || User.IsInRole("Admin")))
            {
                await _ilanService.Sil(id);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var ilan = await _ilanService.GetirById(id);
            if (ilan == null)
            {
                return NotFound();
            }
            return View(ilan);
        }
    }
}
