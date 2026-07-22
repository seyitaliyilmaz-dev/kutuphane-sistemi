using Microsoft.EntityFrameworkCore;
using kutuphane_sistemi.Models;

namespace kutuphane_sistemi;

public class KutuphaneDbContext : DbContext
{
    public KutuphaneDbContext(DbContextOptions<KutuphaneDbContext> options) : base(options) { }

    public DbSet<Yazar> Yazarlar { get; set; }
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Ogrenci> Ogrenciler { get; set; }
    public DbSet<OdunAlma> OdunAlmalar { get; set; }
}