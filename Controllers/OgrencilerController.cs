using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

[Authorize(Roles = "Admin")]
public class OgrencilerController : Controller
{
    private readonly KutuphaneDbContext _context;

    public OgrencilerController(KutuphaneDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var ogrenciler = await _context.Ogrenciler.ToListAsync();
        return View(ogrenciler);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Ogrenci ogrenci)
    {
        if (!ModelState.IsValid)
        {
            return View(ogrenci);
        }

        _context.Ogrenciler.Add(ogrenci);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var ogrenci = await _context.Ogrenciler.FindAsync(id);
        if (ogrenci == null)
        {
            return NotFound();
        }
        return View(ogrenci);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Ogrenci ogrenci)
    {
        if (id != ogrenci.OgrenciID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(ogrenci);
        }

        _context.Update(ogrenci);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var ogrenci = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciID == id);
        if (ogrenci == null)
        {
            return NotFound();
        }
        return View(ogrenci);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ogrenci = await _context.Ogrenciler.FindAsync(id);
        if (ogrenci != null)
        {
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}