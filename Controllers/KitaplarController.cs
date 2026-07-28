using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class KitaplarController : Controller
{
    private readonly KutuphaneDbContext _context;

    public KitaplarController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /Kitaplar (herkese açık)
    public async Task<IActionResult> Index(string? arama)
    {
        var sorgu = _context.Kitaplar.Include(k => k.Yazar).AsQueryable();

        if (!string.IsNullOrWhiteSpace(arama))
        {
            sorgu = sorgu.Where(k =>
                k.Baslik.Contains(arama) ||
                (k.Yazar != null && k.Yazar.AdSoyad.Contains(arama)));
        }

        ViewBag.AramaMetni = arama;

        var kitaplar = await sorgu.ToListAsync();
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