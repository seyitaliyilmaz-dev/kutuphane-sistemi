using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class OdunAlmalarController : Controller
{
    private readonly KutuphaneDbContext _context;

    public OdunAlmalarController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: /OdunAlmalar
    public async Task<IActionResult> Index()
    {
        var odunAlmalar = await _context.OdunAlmalar
            .Include(o => o.Kitap)
            .Include(o => o.Ogrenci)
            .OrderByDescending(o => o.AlisTarihi)
            .ToListAsync();
        return View(odunAlmalar);
    }

    // GET: /OdunAlmalar/Create
    public IActionResult Create()
    {
        // Sadece rafta olan (ödünç verilmemiş) kitapları listele
        ViewBag.Kitaplar = _context.Kitaplar.Where(k => !k.OduncDurumu).ToList();
        ViewBag.Ogrenciler = _context.Ogrenciler.ToList();
        return View();
    }

    // POST: /OdunAlmalar/Create
    [HttpPost]
    public async Task<IActionResult> Create(OdunAlma odunAlma)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Kitaplar = _context.Kitaplar.Where(k => !k.OduncDurumu).ToList();
            ViewBag.Ogrenciler = _context.Ogrenciler.ToList();
            return View(odunAlma);
        }

        odunAlma.AlisTarihi = DateTime.Now;
        odunAlma.SonTeslimTarihi = DateTime.Now.AddDays(14); // 14 gün ödünç süresi
        odunAlma.IadeTarihi = null;

        _context.OdunAlmalar.Add(odunAlma);

        // Kitabın durumunu "ödünçte" yap
        var kitap = await _context.Kitaplar.FindAsync(odunAlma.KitapID);
        if (kitap != null)
        {
            kitap.OduncDurumu = true;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: /OdunAlmalar/IadeEt/5
    [HttpPost]
    public async Task<IActionResult> IadeEt(int id)
    {
        var odunAlma = await _context.OdunAlmalar.FindAsync(id);
        if (odunAlma == null)
        {
            return NotFound();
        }

        odunAlma.IadeTarihi = DateTime.Now;

        // Kitabın durumunu "rafta" yap
        var kitap = await _context.Kitaplar.FindAsync(odunAlma.KitapID);
        if (kitap != null)
        {
            kitap.OduncDurumu = false;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}