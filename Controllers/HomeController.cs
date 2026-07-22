using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class HomeController : Controller
{
    private readonly KutuphaneDbContext _context;

    public HomeController(KutuphaneDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ToplamKitap = await _context.Kitaplar.CountAsync();
        ViewBag.OduncteKitap = await _context.Kitaplar.CountAsync(k => k.OduncDurumu);
        ViewBag.ToplamOgrenci = await _context.Ogrenciler.CountAsync();
        ViewBag.GecikmisKayit = await _context.OdunAlmalar
            .CountAsync(o => o.IadeTarihi == null && o.SonTeslimTarihi < DateTime.Now);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}