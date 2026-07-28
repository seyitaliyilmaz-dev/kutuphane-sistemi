using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;
using System.Security.Claims;

namespace kutuphane_sistemi.Controllers;

[Authorize]
public class OduncTalepleriController : Controller
{
    private readonly KutuphaneDbContext _context;

    public OduncTalepleriController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /OduncTalepleri
    // Admin ise hepsini görür, öğrenci ise sadece kendi taleplerini görür
    public async Task<IActionResult> Index()
    {
        var sorgu = _context.OduncTalepleri
            .Include(t => t.Kitap)
            .Include(t => t.Ogrenci)
            .OrderByDescending(t => t.TalepTarihi)
            .AsQueryable();

        if (!User.IsInRole("Admin"))
        {
            var ogrenciIdClaim = User.FindFirst("OgrenciID");
            if (ogrenciIdClaim == null)
            {
                return Forbid();
            }
            int ogrenciId = int.Parse(ogrenciIdClaim.Value);
            sorgu = sorgu.Where(t => t.OgrenciID == ogrenciId);
        }

        var talepler = await sorgu.ToListAsync();
        return View(talepler);
    }

    // GET: /OduncTalepleri/Create (sadece öğrenci)
    public IActionResult Create()
    {
        if (User.IsInRole("Admin"))
        {
            return Forbid();
        }

        ViewBag.Kitaplar = _context.Kitaplar.Where(k => !k.OduncDurumu).ToList();
        return View();
    }

    // POST: /OduncTalepleri/Create (sadece öğrenci)
    [HttpPost]
    public async Task<IActionResult> Create(OduncTalebi talep)
    {
        if (User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var ogrenciIdClaim = User.FindFirst("OgrenciID");
        if (ogrenciIdClaim == null)
        {
            return Forbid();
        }

        talep.OgrenciID = int.Parse(ogrenciIdClaim.Value);
        talep.TalepTarihi = DateTime.Now;
        talep.Durum = "Beklemede";

        if (!ModelState.IsValid)
        {
            ViewBag.Kitaplar = _context.Kitaplar.Where(k => !k.OduncDurumu).ToList();
            return View(talep);
        }

        _context.OduncTalepleri.Add(talep);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: /OduncTalepleri/Onayla/5 (sadece Admin)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Onayla(int id)
    {
        var talep = await _context.OduncTalepleri.FindAsync(id);
        if (talep == null)
        {
            return NotFound();
        }

        var kitap = await _context.Kitaplar.FindAsync(talep.KitapID);
        if (kitap == null || kitap.OduncDurumu)
        {
            TempData["Hata"] = "Bu kitap artık uygun değil.";
            return RedirectToAction(nameof(Index));
        }

        talep.Durum = "Onaylandi";
        kitap.OduncDurumu = true;

        var odunAlma = new OdunAlma
        {
            KitapID = talep.KitapID,
            OgrenciID = talep.OgrenciID,
            AlisTarihi = DateTime.Now,
            SonTeslimTarihi = DateTime.Now.AddDays(14),
            IadeTarihi = null
        };
        _context.OdunAlmalar.Add(odunAlma);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: /OduncTalepleri/Reddet/5 (sadece Admin)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Reddet(int id)
    {
        var talep = await _context.OduncTalepleri.FindAsync(id);
        if (talep == null)
        {
            return NotFound();
        }

        talep.Durum = "Reddedildi";
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}