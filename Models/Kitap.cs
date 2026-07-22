using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class Kitap
{
    public int KitapID { get; set; }

    [Required(ErrorMessage = "Kitap başlığı zorunludur.")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    public string Baslik { get; set; } = string.Empty;

    public bool OduncDurumu { get; set; } = false;

    [Required(ErrorMessage = "Lütfen bir yazar seçin.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir yazar seçin.")]
    public int YazarID { get; set; }
    public Yazar? Yazar { get; set; }
}