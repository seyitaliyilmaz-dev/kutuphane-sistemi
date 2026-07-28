using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

[Authorize]
public class OdunAlmalarController : Controller
{
    private readonly KutuphaneDbContext _context;

    public OdunAlmalarController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /OdunAlmalar (sadece Admin - tüm kayıtlar)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var odunAlmalar = await _context.OdunAlmalar
            .Include(o => o.Kitap)
            .Include(o => o.Ogrenci)
            .OrderByDescending(o => o.AlisTarihi)
            .ToListAsync();
        return View(odunAlmalar);
    }

    // GET: /OdunAlmalar/Gecmisim (sadece Öğrenci - kendi kayıtları)
    public async Task<IActionResult> Gecmisim()
    {
        var ogrenciIdClaim = User.FindFirst("OgrenciID");
        if (ogrenciIdClaim == null)
        {
            return Forbid();
        }
        int ogrenciId = int.Parse(ogrenciIdClaim.Value);

        var kayitlarim = await _context.OdunAlmalar
            .Include(o => o.Kitap)
            .Where(o => o.OgrenciID == ogrenciId)
            .OrderByDescending(o => o.AlisTarihi)
            .ToListAsync();

        return View(kayitlarim);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> IadeEt(int id)
    {
        var odunAlma = await _context.OdunAlmalar.FindAsync(id);
        if (odunAlma == null)
        {
            return NotFound();
        }

        odunAlma.IadeTarihi = DateTime.Now;

        var kitap = await _context.Kitaplar.FindAsync(odunAlma.KitapID);
        if (kitap != null)
        {
            kitap.OduncDurumu = false;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}