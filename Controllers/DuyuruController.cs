using Microsoft.AspNetCore.Mvc;
using SmartCityHub.Models;
using System.Security.Claims;

namespace SmartCityHub.Controllers
{
    public class DuyuruController : Controller
    {
        private readonly DuyuruService _duyuruService;

        public DuyuruController(DuyuruService duyuruService)
        {
            _duyuruService = duyuruService;
        }

        public async Task<IActionResult> Index()
        {
            var duyurular = await _duyuruService.HepsiniGetir();
            return View(duyurular);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Duyuru yeniDuyuru, IFormFile? Resim)
        {
             var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                if (Resim != null && Resim.Length > 0)
                {
                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(Resim.FileName);
                    var yol = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        dosyaAdi
                    );

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        await Resim.CopyToAsync(stream);
                    }

                    yeniDuyuru.ResimUrl = "/uploads/" + dosyaAdi;
                }

                await _duyuruService.Ekle(yeniDuyuru,kullaniciId);
                return RedirectToAction(nameof(Index));
            }
            return View(yeniDuyuru);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(string id)
        {
            await _duyuruService.Sil(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var duyuru = await _duyuruService.GetirById(id);
            if (duyuru == null)
            {
                return NotFound();
            }
            return View(duyuru);
        }

        [HttpGet]
        public async Task<IActionResult> Guncelle(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var duyuru = await _duyuruService.GetirById(id);
            if (duyuru == null)
            {
                return NotFound();
            }
            return View(duyuru);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Duyuru guncelDuyuru, IFormFile? Resim)
        {
            var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                if (Resim != null && Resim.Length > 0)
                {
                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(Resim.FileName);
                    var yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", dosyaAdi);

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        await Resim.CopyToAsync(stream);
                    }
                    guncelDuyuru.ResimUrl = "/uploads/" + dosyaAdi;
                }
                await _duyuruService.Guncelle(guncelDuyuru);
                return RedirectToAction("Index");
            }
            return View(guncelDuyuru);
        }
    }
}
