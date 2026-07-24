using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class Yazar
{
    public int YazarID { get; set; }

    [Required(ErrorMessage = "Yazar adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir.")]
    public string AdSoyad { get; set; } = string.Empty;

    public List<Kitap>? Kitaplar { get; set; }
}