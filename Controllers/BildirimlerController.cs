using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

[Authorize(Roles = "Admin")]
public class BildirimlerController : Controller
{
    private readonly KutuphaneDbContext _context;

    public BildirimlerController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /Bildirimler
    public async Task<IActionResult> Index()
    {
        var bildirimler = await _context.Bildirimler
            .OrderByDescending(b => b.OlusturmaTarihi)
            .Take(50)
            .ToListAsync();
        return View(bildirimler);
    }

    // GET: /Bildirimler/OkunmamisSayisi (JSON, menüdeki rozet için)
    [HttpGet]
    public async Task<IActionResult> OkunmamisSayisi()
    {
        var sayi = await _context.Bildirimler.CountAsync(b => !b.Okundu);
        return Json(new { sayi });
    }

    // POST: /Bildirimler/TumunuOkunduYap
    [HttpPost]
    public async Task<IActionResult> TumunuOkunduYap()
    {
        var okunmamislar = await _context.Bildirimler.Where(b => !b.Okundu).ToListAsync();
        foreach (var b in okunmamislar)
        {
            b.Okundu = true;
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}