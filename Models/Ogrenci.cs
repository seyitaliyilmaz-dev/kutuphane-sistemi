using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class Ogrenci
{
    public int OgrenciID { get; set; }

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    public string AdSoyad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Öğrenci numarası zorunludur.")]
    [StringLength(20, ErrorMessage = "Öğrenci numarası en fazla 20 karakter olabilir.")]
    public string OgrenciNo { get; set; } = string.Empty;
}