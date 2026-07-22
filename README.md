# Kütüphane Kitap Takip Sistemi

ASP.NET Core MVC ve Entity Framework Core (Code First) kullanılarak geliştirilmiş bir kütüphane yönetim sistemi.

## Özellikler

- **Kitap Yönetimi:** Kitap ekleme, düzenleme, silme; yazar ilişkilendirme
- **Öğrenci Yönetimi:** Öğrenci kayıtlarının CRUD işlemleri
- **Ödünç Alma/Verme Sistemi:** Kitap ödünç alma, otomatik 14 günlük son teslim tarihi hesaplama, iade işlemi
- **Geç Teslim Takibi:** Süresi geçmiş ve iade edilmemiş kayıtların otomatik tespiti ve vurgulanması
- **Yönetim Paneli (Dashboard):** Toplam kitap, ödünçte olan kitap, öğrenci ve gecikmiş kayıt sayılarının anlık özeti
- **Veri Doğrulama:** DataAnnotations ile form validasyonu (zorunlu alanlar, karakter sınırları)

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core (Code First / Migrations)
- Microsoft SQL Server
- Razor View Engine
- Bootstrap

## Veri Modeli

- **Yazar** ↔ **Kitap**: Bir yazarın birden fazla kitabı olabilir
- **Kitap** ↔ **Öğrenci** (OdunAlma tablosu üzerinden): Bir öğrenci birden fazla kitap ödünç alabilir, bir kitap zaman içinde birden fazla öğrenciye ödünç verilebilir

## Çalıştırma

```bash
dotnet restore
dotnet ef database update
dotnet run
```

