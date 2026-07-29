using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class KategorilerController : Controller
{
    private readonly KutuphaneDbContext _context;

    public KategorilerController(KutuphaneDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var kategoriler = await _context.Kategoriler.Include(k => k.Kitaplar).ToListAsync();
        return View(kategoriler);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Kategori kategori)
    {
        if (!ModelState.IsValid)
        {
            return View(kategori);
        }

        _context.Kategoriler.Add(kategori);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var kategori = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .FirstOrDefaultAsync(k => k.KategoriID == id);
        if (kategori == null)
        {
            return NotFound();
        }
        return View(kategori);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var kategori = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .FirstOrDefaultAsync(k => k.KategoriID == id);

        if (kategori != null)
        {
            if (kategori.Kitaplar != null && kategori.Kitaplar.Any())
            {
                TempData["Hata"] = "Bu kategoride kitaplar var, önce onları silin veya başka kategoriye taşıyın.";
                return RedirectToAction(nameof(Index));
            }

            _context.Kategoriler.Remove(kategori);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}