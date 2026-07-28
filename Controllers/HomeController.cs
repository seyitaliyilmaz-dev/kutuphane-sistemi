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

        // Grafik 1: Kitap durumu (Rafta / Ödünçte)
        int raftaSayisi = await _context.Kitaplar.CountAsync(k => !k.OduncDurumu);
        int oduncteSayisi = await _context.Kitaplar.CountAsync(k => k.OduncDurumu);
        ViewBag.KitapDurumEtiketleri = new[] { "Rafta", "Ödünçte" };
        ViewBag.KitapDurumSayilari = new[] { raftaSayisi, oduncteSayisi };

        // Grafik 2: Yazar başına kitap sayısı (en çok 5 yazar)
        var yazarKitapSayilari = await _context.Yazarlar
            .Select(y => new { y.AdSoyad, KitapSayisi = y.Kitaplar!.Count })
            .OrderByDescending(y => y.KitapSayisi)
            .Take(5)
            .ToListAsync();

        ViewBag.YazarEtiketleri = yazarKitapSayilari.Select(y => y.AdSoyad).ToArray();
        ViewBag.YazarKitapSayilari = yazarKitapSayilari.Select(y => y.KitapSayisi).ToArray();

        // Grafik 3: Talep durumları
        int beklemede = await _context.OduncTalepleri.CountAsync(t => t.Durum == "Beklemede");
        int onaylandi = await _context.OduncTalepleri.CountAsync(t => t.Durum == "Onaylandi");
        int reddedildi = await _context.OduncTalepleri.CountAsync(t => t.Durum == "Reddedildi");
        ViewBag.TalepEtiketleri = new[] { "Beklemede", "Onaylandı", "Reddedildi" };
        ViewBag.TalepSayilari = new[] { beklemede, onaylandi, reddedildi };

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