using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class Kullanici
{
    public int KullaniciID { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50)]
    public string KullaniciAdi { get; set; } = string.Empty;

    [Required]
    public string SifreHash { get; set; } = string.Empty;

    [Required]
    public string Rol { get; set; } = "Ogrenci"; // "Admin" veya "Ogrenci"

    // Öğrenci ise, hangi Ogrenci kaydına bağlı olduğu
    public int? OgrenciID { get; set; }
    public Ogrenci? Ogrenci { get; set; }
}