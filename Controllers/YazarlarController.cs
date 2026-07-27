using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class YazarlarController : Controller
{
    private readonly KutuphaneDbContext _context;

    public YazarlarController(KutuphaneDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var yazarlar = await _context.Yazarlar.Include(y => y.Kitaplar).ToListAsync();
        return View(yazarlar);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Yazar yazar)
    {
        if (!ModelState.IsValid)
        {
            return View(yazar);
        }

        _context.Yazarlar.Add(yazar);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var yazar = await _context.Yazarlar.FindAsync(id);
        if (yazar == null)
        {
            return NotFound();
        }
        return View(yazar);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Yazar yazar)
    {
        if (id != yazar.YazarID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(yazar);
        }

        _context.Update(yazar);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var yazar = await _context.Yazarlar
            .Include(y => y.Kitaplar)
            .FirstOrDefaultAsync(y => y.YazarID == id);
        if (yazar == null)
        {
            return NotFound();
        }
        return View(yazar);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var yazar = await _context.Yazarlar
            .Include(y => y.Kitaplar)
            .FirstOrDefaultAsync(y => y.YazarID == id);

        if (yazar != null)
        {
            if (yazar.Kitaplar != null && yazar.Kitaplar.Any())
            {
                TempData["Hata"] = "Bu yazarın kitapları olduğu için silinemez. Önce kitaplarını silin.";
                return RedirectToAction(nameof(Index));
            }

            _context.Yazarlar.Remove(yazar);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}