namespace kutuphane_sistemi.Models;

public class Yazar
{
    public int YazarID { get; set; }
    public string AdSoyad { get; set; } = string.Empty;

    // Bir yazarın birden fazla kitabı olabilir
    public List<Kitap>? Kitaplar { get; set; }
}