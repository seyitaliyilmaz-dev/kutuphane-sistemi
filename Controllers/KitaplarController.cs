using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi.Controllers;

public class KitaplarController : Controller
{
    private readonly KutuphaneDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private const int SayfaBasinaKayit = 8;

    public KitaplarController(KutuphaneDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index(string? arama, int sayfa = 1, string sirala = "baslik_az", int? kategoriId = null)
    {
        var sorgu = _context.Kitaplar.Include(k => k.Yazar).Include(k => k.Kategori).AsQueryable();

        if (!string.IsNullOrWhiteSpace(arama))
        {
            sorgu = sorgu.Where(k =>
                k.Baslik.Contains(arama) ||
                (k.Yazar != null && k.Yazar.AdSoyad.Contains(arama)));
        }

        if (kategoriId.HasValue && kategoriId.Value > 0)
        {
            sorgu = sorgu.Where(k => k.KategoriID == kategoriId.Value);
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
        int toplamSayfa = Math.Max(1, (int)Math.Ceiling(toplamKayit / (double)SayfaBasinaKayit));

        if (sayfa < 1) sayfa = 1;
        if (sayfa > toplamSayfa) sayfa = toplamSayfa;

        var kitaplar = await sorgu
            .Skip((sayfa - 1) * SayfaBasinaKayit)
            .Take(SayfaBasinaKayit)
            .ToListAsync();

        ViewBag.AramaMetni = arama ?? "";
        ViewBag.MevcutSayfa = sayfa;
        ViewBag.ToplamSayfa = toplamSayfa;
        ViewBag.Sirala = sirala;
        ViewBag.KategoriId = kategoriId ?? 0;
        ViewBag.Kategoriler = _context.Kategoriler.ToList();

        return View(kitaplar);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Yazarlar = _context.Yazarlar.ToList();
        ViewBag.Kategoriler = _context.Kategoriler.ToList();
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Kitap kitap, IFormFile? resimDosyasi)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Yazarlar = _context.Yazarlar.ToList();
            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(kitap);
        }

        if (resimDosyasi != null && resimDosyasi.Length > 0)
        {
            kitap.ResimYolu = await ResmiKaydet(resimDosyasi);
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
        ViewBag.Kategoriler = _context.Kategoriler.ToList();
        return View(kitap);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Kitap kitap, IFormFile? resimDosyasi)
    {
        if (id != kitap.KitapID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Yazarlar = _context.Yazarlar.ToList();
            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(kitap);
        }

        var mevcutKitap = await _context.Kitaplar.AsNoTracking().FirstOrDefaultAsync(k => k.KitapID == id);

        if (resimDosyasi != null && resimDosyasi.Length > 0)
        {
            kitap.ResimYolu = await ResmiKaydet(resimDosyasi);
        }
        else
        {
            kitap.ResimYolu = mevcutKitap?.ResimYolu;
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

    private async Task<string> ResmiKaydet(IFormFile dosya)
    {
        var uzanti = Path.GetExtension(dosya.FileName);
        var dosyaAdi = $"{Guid.NewGuid()}{uzanti}";
        var klasorYolu = Path.Combine(_environment.WebRootPath, "kitap-resimleri");

        if (!Directory.Exists(klasorYolu))
        {
            Directory.CreateDirectory(klasorYolu);
        }

        var tamYol = Path.Combine(klasorYolu, dosyaAdi);

        using (var stream = new FileStream(tamYol, FileMode.Create))
        {
            await dosya.CopyToAsync(stream);
        }

        return $"/kitap-resimleri/{dosyaAdi}";
    }
}