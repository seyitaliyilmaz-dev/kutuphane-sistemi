# Kütüphane Kitap Takip Sistemi

ASP.NET Core MVC ve Entity Framework Core (Code First) kullanılarak geliştirilmiş bir kütüphane yönetim sistemi.

## Özellikler

- **Kitap Yönetimi:** Kitap ekleme, düzenleme, silme; yazar ilişkilendirme; başlık/yazar adına göre arama
- **Yazar Yönetimi:** Yazar ekleme, düzenleme, silme; kitapları olan bir yazarın silinmesini engelleyen veri bütünlüğü kontrolü
- **Öğrenci Yönetimi:** Öğrenci kayıtlarının CRUD işlemleri
- **Ödünç Alma/Verme Sistemi:** Kitap ödünç alma, otomatik 14 günlük son teslim tarihi hesaplama, iade işlemi
- **Geç Teslim Takibi:** Süresi geçmiş ve iade edilmemiş kayıtların otomatik tespiti ve vurgulanması
- **Yönetim Paneli (Dashboard):** Toplam kitap, ödünçte olan kitap, öğrenci ve gecikmiş kayıt sayılarının anlık özeti
- **Veri Doğrulama:** DataAnnotations ile form validasyonu (zorunlu alanlar, karakter sınırları)
- **Kimlik Doğrulama:** Cookie tabanlı kütüphaneci girişi; listeleme herkese açık, ekleme/düzenleme/silme sadece giriş yapan kullanıcıya açık

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core (Code First / Migrations)
- Microsoft SQL Server
- Razor View Engine
- Cookie Authentication
- Bootstrap

## Veri Modeli

- **Yazar** ↔ **Kitap**: Bir yazarın birden fazla kitabı olabilir
- **Kitap** ↔ **Öğrenci** (OdunAlma tablosu üzerinden): Bir öğrenci birden fazla kitap ödünç alabilir, bir kitap zaman içinde birden fazla öğrenciye ödünç verilebilir

## Modüller

| Modül | İşlemler |
|---|---|
| Kitaplar | Listele (herkese açık), Ara, Ekle/Düzenle/Sil (giriş gerekli) |
| Yazarlar | Listele (herkese açık), Ekle/Düzenle/Sil (giriş gerekli, kitapları varsa silme engellenir) |
| Öğrenciler | Listele (herkese açık), Ekle/Düzenle/Sil (giriş gerekli) |
| Ödünç Almalar | Listele (herkese açık), Ödünç ver/İade al (giriş gerekli), Gecikme takibi |

## Çalıştırma

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## Not

Bu proje bir öğrenme/staj çalışmasıdır. Giriş bilgileri (kullanıcı adı/şifre) demo amaçlı sabit kodlanmıştır; gerçek bir üretim ortamında kullanıcı bilgileri veritabanında şifrelenmiş (hash'lenmiş) olarak saklanmalı ve ASP.NET Core Identity gibi bir kütüphane kullanılmalıdır.

