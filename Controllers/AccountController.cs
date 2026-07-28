using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class AccountController : Controller
{
    private readonly KutuphaneDbContext _context;

    public AccountController(KutuphaneDbContext context)
    {
        _context = context;
    }

    public class GirisBilgisi
    {
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }

    public class KayitBilgisi
    {
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string OgrenciNo { get; set; } = string.Empty;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(GirisBilgisi giris)
    {
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.KullaniciAdi == giris.KullaniciAdi);

        if (kullanici == null || !SifreYardimcisi.Dogrula(giris.Sifre, kullanici.SifreHash))
        {
            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View(giris);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
            new Claim(ClaimTypes.Role, kullanici.Rol)
        };

        if (kullanici.OgrenciID.HasValue)
        {
            claims.Add(new Claim("OgrenciID", kullanici.OgrenciID.Value.ToString()));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(KayitBilgisi kayit)
    {
        var varOlanKullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.KullaniciAdi == kayit.KullaniciAdi);

        if (varOlanKullanici != null)
        {
            ViewBag.Hata = "Bu kullanıcı adı zaten alınmış.";
            return View(kayit);
        }

        var ogrenci = new Ogrenci
        {
            AdSoyad = kayit.AdSoyad,
            OgrenciNo = kayit.OgrenciNo
        };
        _context.Ogrenciler.Add(ogrenci);
        await _context.SaveChangesAsync();

        var kullanici = new Kullanici
        {
            KullaniciAdi = kayit.KullaniciAdi,
            SifreHash = SifreYardimcisi.Hashle(kayit.Sifre),
            Rol = "Ogrenci",
            OgrenciID = ogrenci.OgrenciID
        };
        _context.Kullanicilar.Add(kullanici);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult ErisimReddedildi()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}