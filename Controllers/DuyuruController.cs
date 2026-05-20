using Microsoft.AspNetCore.Mvc;
using SmartCityHub.Models;

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

                await _duyuruService.Ekle(yeniDuyuru);
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

        public async Task<IActionResult> GetirById(string id)
        {
            var duyuru = await _duyuruService.GetirById(id);
            if (duyuru == null)
                return NotFound();

            return View(duyuru);
        }
    }
}
