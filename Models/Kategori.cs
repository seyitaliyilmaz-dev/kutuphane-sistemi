using System.ComponentModel.DataAnnotations;

namespace kutuphane_sistemi.Models;

public class Kategori
{
    public int KategoriID { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [StringLength(50)]
    public string Ad { get; set; } = string.Empty;

    public List<Kitap>? Kitaplar { get; set; }
}