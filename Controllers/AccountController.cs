using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace kutuphane_sistemi.Controllers;

public class AccountController : Controller
{
    private const string SabitKullaniciAdi = "kutuphaneci";
    private const string SabitSifre = "1234";

    public class GirisBilgisi
    {
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(GirisBilgisi giris)
    {
        if (giris.KullaniciAdi != SabitKullaniciAdi || giris.Sifre != SabitSifre)
        {
            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View(giris);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, giris.KullaniciAdi)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}