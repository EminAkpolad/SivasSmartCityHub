using Microsoft.AspNetCore.Mvc;
using SmartCityHub.Models;
using System.Security.Claims;

namespace SmartCityHub.Controllers
{
    public class OneriController : Controller
    {
        private readonly OneriService _oneriService;

        public OneriController(OneriService oneriService)
        {
            _oneriService=oneriService;
        }

        public async Task<IActionResult> Index()
        {
            var oneriler=await _oneriService.HepsiniGetir();
            return View(oneriler);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Oneri YeniOneri)
        {
              var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                await _oneriService.Ekle(YeniOneri,kullaniciId);
                return RedirectToAction(nameof(Index));
            }
            return View(YeniOneri);
        }
        [HttpPost]
        public async Task<IActionResult> Sil(string id)
        {
            await _oneriService.Sil(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Guncelle(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var oneri=await _oneriService.GetirById(id);
            if (oneri == null)
            {
                return NotFound();
            }
            return View(oneri);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Oneri guncelOneri)
        {
            if (ModelState.IsValid)
            {
                await _oneriService.Guncelle(guncelOneri);
                return RedirectToAction("Index");
            }
            return View(guncelOneri);
        }
    }
}