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
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<OduncTalebi> OduncTalepleri { get; set; }
    public DbSet<Bildirim> Bildirimler { get; set; }
    public DbSet<Kategori> Kategoriler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ogrenci>()
            .HasIndex(o => o.OgrenciNo)
            .IsUnique();

        modelBuilder.Entity<Kullanici>()
            .HasIndex(k => k.KullaniciAdi)
            .IsUnique();
    }
}