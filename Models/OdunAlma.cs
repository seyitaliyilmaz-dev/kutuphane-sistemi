using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class OdunAlma
{
    public int OdunAlmaID { get; set; }

    [Required(ErrorMessage = "Lütfen bir kitap seçin.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kitap seçin.")]
    public int KitapID { get; set; }
    public Kitap? Kitap { get; set; }

    [Required(ErrorMessage = "Lütfen bir öğrenci seçin.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir öğrenci seçin.")]
    public int OgrenciID { get; set; }
    public Ogrenci? Ogrenci { get; set; }

    public DateTime AlisTarihi { get; set; } = DateTime.Now;
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
}