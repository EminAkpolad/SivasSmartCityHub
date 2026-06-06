using DnsClient.Protocol;
using Microsoft.AspNetCore.Mvc;
using SmartCityHub.Models;
using System.Security.Claims;

namespace SmartCityHub.Controllers
{
    public class EtkinlikController : Controller
    {
        private readonly EtkinlikService _etkinlikService;

        public EtkinlikController(EtkinlikService etkinlikService)
        {
            _etkinlikService=etkinlikService;
        }

        public async Task<IActionResult> Index(){
            var etkinlikler=await _etkinlikService.HepsiniGetir();
            return View(etkinlikler);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Etkinlik yeniEtkinlik,IFormFile? Resim)
        {
            var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                if(Resim!=null && Resim.Length > 0)
                {
                    var dosyaAdi=Guid.NewGuid().ToString()+Path.GetExtension(Resim.FileName);
                    var yol=Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        dosyaAdi
                    );

                    using(var stream=new FileStream(yol, FileMode.Create))
                    {
                        await Resim.CopyToAsync(stream);
                    }

                    yeniEtkinlik.ResimUrl="/uploads/"+dosyaAdi;
                }

                await _etkinlikService.Ekle(yeniEtkinlik,kullaniciId);
                return RedirectToAction("Index");
            }
            return View(yeniEtkinlik);
        }

        [HttpPost]
        public async Task<IActionResult> Detay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var etkinlik=await _etkinlikService.GetirById(id);
            if (etkinlik == null)
            {
                return NotFound();
            }
            return View(etkinlik);
        }

        [HttpGet]
        public async Task<IActionResult> Guncelle(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var etkinlik=await _etkinlikService.GetirById(id);
            if (etkinlik == null)
            {
                return NotFound();
            }
            return View(etkinlik);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Etkinlik guncelEtkinlik,IFormFile? Resim)
        {
            var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (kullaniciId == null) return RedirectToAction("Giris", "Hesap");
            if (ModelState.IsValid)
            {
                if(Resim!=null && Resim.Length > 0)
                {
                    var dosyaAdi=Guid.NewGuid().ToString() + Path.GetExtension(Resim.FileName);
                    var yol=Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot",
                     "uploads",
                     dosyaAdi   
                    );

                    using (var stream=new FileStream(yol, FileMode.Create))
                    {
                        await Resim.CopyToAsync(stream);
                    }
                    guncelEtkinlik.ResimUrl="/uploads/"+dosyaAdi;
                }
                await _etkinlikService.Guncelle(guncelEtkinlik);
                return RedirectToAction("Index");
            }
            return View(guncelEtkinlik);
        }
    }
}