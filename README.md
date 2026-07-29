# Kütüphane Kitap Takip Sistemi

ASP.NET Core MVC ve Entity Framework Core (Code First) kullanılarak geliştirilmiş, rol tabanlı bir kütüphane yönetim sistemi.

## Roller

- **Admin (Kütüphaneci):** Kitap, yazar ve öğrenci kayıtlarını yönetir; ödünç taleplerini onaylar/reddeder
- **Öğrenci:** Kendi hesabını oluşturur, kitaplara ödünç talebi gönderir, taleplerinin durumunu takip eder

## Özellikler

- **Kitap Yönetimi:** Kitap ekleme, düzenleme, silme (Admin); yazar ilişkilendirme; başlık/yazar adına göre arama
- **Yazar Yönetimi:** Yazar ekleme, düzenleme, silme (Admin); kitapları olan bir yazarın silinmesini engelleyen veri bütünlüğü kontrolü
- **Öğrenci Yönetimi:** Öğrenci kayıtlarının CRUD işlemleri (Admin)
- **Kullanıcı Kaydı:** Öğrenciler kendi hesaplarını oluşturabilir (kullanıcı adı, şifre, ad soyad, öğrenci no)
- **Ödünç Talep Sistemi:** Öğrenci bir kitap için talep oluşturur; Admin onaylar veya reddeder; onaylanan talep otomatik olarak gerçek bir ödünç alma kaydına dönüşür ve kitabın durumunu günceller
- **Geç Teslim Takibi:** Süresi geçmiş ve iade edilmemiş kayıtların otomatik tespiti ve vurgulanması
- **Yönetim Paneli (Dashboard):** Toplam kitap, ödünçte olan kitap, öğrenci ve gecikmiş kayıt sayılarının anlık özeti
- **Veri Doğrulama:** DataAnnotations ile form validasyonu
- **Rol Tabanlı Yetkilendirme:** Cookie tabanlı kimlik doğrulama, Admin ve Öğrenci rollerine göre farklı erişim yetkileri

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core (Code First / Migrations)
- Microsoft SQL Server
- Razor View Engine
- Cookie Authentication + Role-based Authorization
- Bootstrap

## Veri Modeli

- **Yazar** ↔ **Kitap**: Bir yazarın birden fazla kitabı olabilir
- **Kitap** ↔ **Öğrenci** (OdunAlma tablosu üzerinden): Bir öğrenci birden fazla kitap ödünç alabilir
- **Kullanici** ↔ **Ogrenci**: Her öğrenci kullanıcı hesabı, bir Ogrenci kaydına bağlıdır
- **OduncTalebi**: Öğrencinin kitap talebini, onay durumunu ve tarihini tutar

## İş Akışı: Ödünç Alma Süreci
## Modüller

| Modül | Erişim |
|---|---|
| Kitaplar | Listele/Ara: herkese açık — Ekle/Düzenle/Sil: Admin |
| Yazarlar | Listele: herkese açık — Ekle/Düzenle/Sil: Admin |
| Öğrenciler | Tümü: Admin |
| Ödünç Talepleri | Oluştur: Öğrenci — Onayla/Reddet: Admin |
| Ödünç Almalar | Listele/İade Al: Admin |

## Çalıştırma

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## Not

Bu proje bir öğrenme/staj çalışmasıdır. Şifre hash'leme SHA256 ile basitleştirilmiştir; gerçek bir üretim ortamında ASP.NET Core Identity veya BCrypt gibi salt destekli, endüstri standardı bir çözüm kullanılmalıdır.


## Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

## Geliştirici

Seyit Ali Yılmaz

