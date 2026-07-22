using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class OgrencilerController : Controller
{
    private readonly KutuphaneDbContext _context;

    public OgrencilerController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /Ogrenciler
    public async Task<IActionResult> Index()
    {
        var ogrenciler = await _context.Ogrenciler.ToListAsync();
        return View(ogrenciler);
    }

    // GET: /Ogrenciler/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Ogrenciler/Create
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

    // GET: /Ogrenciler/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ogrenci = await _context.Ogrenciler.FindAsync(id);
        if (ogrenci == null)
        {
            return NotFound();
        }
        return View(ogrenci);
    }

    // POST: /Ogrenciler/Edit/5
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

    // GET: /Ogrenciler/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var ogrenci = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciID == id);
        if (ogrenci == null)
        {
            return NotFound();
        }
        return View(ogrenci);
    }

    // POST: /Ogrenciler/Delete/5
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