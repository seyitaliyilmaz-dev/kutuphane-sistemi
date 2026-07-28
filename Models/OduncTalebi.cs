using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class OduncTalebi
{
    public int OduncTalebiID { get; set; }

    [Required(ErrorMessage = "Lütfen bir kitap seçin.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kitap seçin.")]
    public int KitapID { get; set; }
    public Kitap? Kitap { get; set; }

    [Required]
    public int OgrenciID { get; set; }
    public Ogrenci? Ogrenci { get; set; }

    public DateTime TalepTarihi { get; set; } = DateTime.Now;

    // "Beklemede", "Onaylandi", "Reddedildi"
    public string Durum { get; set; } = "Beklemede";
}