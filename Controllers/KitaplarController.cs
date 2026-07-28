using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class KitaplarController : Controller
{
    private readonly KutuphaneDbContext _context;
    private const int SayfaBasinaKayit = 5;

    public KitaplarController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /Kitaplar
    public async Task<IActionResult> Index(string? arama, int sayfa = 1, string sirala = "baslik_az")
    {
        var sorgu = _context.Kitaplar.Include(k => k.Yazar).AsQueryable();

        if (!string.IsNullOrWhiteSpace(arama))
        {
            sorgu = sorgu.Where(k =>
                k.Baslik.Contains(arama) ||
                (k.Yazar != null && k.Yazar.AdSoyad.Contains(arama)));
        }

        sorgu = sirala switch
        {
            "baslik_az" => sorgu.OrderBy(k => k.Baslik),
            "baslik_za" => sorgu.OrderByDescending(k => k.Baslik),
            "durum_rafta_once" => sorgu.OrderBy(k => k.OduncDurumu).ThenBy(k => k.Baslik),
            "durum_oduncte_once" => sorgu.OrderByDescending(k => k.OduncDurumu).ThenBy(k => k.Baslik),
            _ => sorgu.OrderBy(k => k.Baslik)
        };

        int toplamKayit = await sorgu.CountAsync();
        int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)SayfaBasinaKayit);

        if (sayfa < 1) sayfa = 1;
        if (toplamSayfa > 0 && sayfa > toplamSayfa) sayfa = toplamSayfa;

        var kitaplar = await sorgu
            .Skip((sayfa - 1) * SayfaBasinaKayit)
            .Take(SayfaBasinaKayit)
            .ToListAsync();

        ViewBag.AramaMetni = arama;
        ViewBag.MevcutSayfa = sayfa;
        ViewBag.ToplamSayfa = toplamSayfa;
        ViewBag.Sirala = sirala;

        return View(kitaplar);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Yazarlar = _context.Yazarlar.ToList();
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Kitap kitap)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Yazarlar = _context.Yazarlar.ToList();
            return View(kitap);
        }

        _context.Kitaplar.Add(kitap);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap == null)
        {
            return NotFound();
        }
        ViewBag.Yazarlar = _context.Yazarlar.ToList();
        return View(kitap);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Kitap kitap)
    {
        if (id != kitap.KitapID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Yazarlar = _context.Yazarlar.ToList();
            return View(kitap);
        }

        _context.Update(kitap);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var kitap = await _context.Kitaplar.Include(k => k.Yazar)
            .FirstOrDefaultAsync(k => k.KitapID == id);
        if (kitap == null)
        {
            return NotFound();
        }
        return View(kitap);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap != null)
        {
            _context.Kitaplar.Remove(kitap);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}