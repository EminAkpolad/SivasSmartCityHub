using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using SmartCityHub.Models;
using SmartCityHub.Services;
namespace SmartCityHub.Controllers
{
	public class HesapController : Controller
	{
		private readonly IMongoCollection<Kullanici> _kullanicilar;
		private readonly IMongoCollection<OgrenciProfil> _ogrenciler;

		private readonly IlanService _ilanService;
		private readonly OneriService _oneriService;

		public HesapController(MongoDbService mongoDbService,IlanService ilanService, OneriService oneriService)
		{
			_kullanicilar = mongoDbService.Kullanicilar;
			_ogrenciler = mongoDbService.OgrenciProfilleri;
			_ilanService = ilanService;
			_oneriService = oneriService;
		}

		[HttpGet]
		public IActionResult Kayit()
		{
			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Kayit(Kullanici yeniKullanici, string Bolum)
		{
			if (string.IsNullOrEmpty(yeniKullanici.KullaniciAdi) ||
				string.IsNullOrEmpty(yeniKullanici.Email) ||
				string.IsNullOrEmpty(yeniKullanici.Password) ||
				string.IsNullOrEmpty(Bolum))
			{
				ViewBag.Hata = "Bu Alanlar Boş Bırakılamaz";
				return View(yeniKullanici);
			}

			var emailKontrol = await _kullanicilar.Find(k => k.Email == yeniKullanici.Email).FirstOrDefaultAsync();
			if (emailKontrol != null)
			{
				ViewBag.Hata = "Bu Email Zaten Kayıtlı";
				return View(yeniKullanici);
			}

			yeniKullanici.CreateAt = DateTime.Now;
			yeniKullanici.Aktiflik = true;
			await _kullanicilar.InsertOneAsync(yeniKullanici);

			var yeniProfil = new OgrenciProfil
			{
				KullaniciId = yeniKullanici.Id,
				Bolum = Bolum,
				Seviye = 1,
				IlgiAlanlari = new List<string>(),
				Hakkinda = "Yeni Üye"
			};
			await _ogrenciler.InsertOneAsync(yeniProfil);
			return RedirectToAction("Giris");
		}

		[HttpGet]
		public IActionResult Giris()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Giris(string Email, string Password)
		{
			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
			{
				ViewBag.Hata = "Email ve Şifre Boş Bırakılamaz";
				return View();
			}

			var kullanici = await _kullanicilar.Find(k => k.Email == Email && k.Password == Password).FirstOrDefaultAsync();

			if (kullanici != null && kullanici.Password == Password)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier,kullanici.Id),
					new Claim(ClaimTypes.Name,kullanici.KullaniciAdi),
					new Claim(ClaimTypes.Email,kullanici.Email),
					new Claim(ClaimTypes.Role,"Ogrenci")
				};
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
				return RedirectToAction("Index", "Home");
			}
			ViewBag.Hata = "Email veya Şifre Hatalı";
			return View();
		}

		public async Task<IActionResult> Cikis()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}


		[HttpGet]
		public async Task<IActionResult> Profilim()
		{
			var kullaniciId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (kullaniciId == null)
			{
				return RedirectToAction("Giris");
			}

			var kullanici = await _kullanicilar.Find(k => k.Id == kullaniciId).FirstOrDefaultAsync();
			var profil = await _ogrenciler.Find(p => p.KullaniciId == kullaniciId).FirstOrDefaultAsync();

			ViewBag.Ilanlarim = await _ilanService.KullanicininIlanlariniGetir(kullaniciId);
			ViewBag.Onerilerim = await _oneriService.KullanicininOnerileriniGetir(kullaniciId);
			ViewBag.Kullanici = kullanici;


			ViewBag.Kullanici = kullanici;
			return View(profil);
		}
	}
}