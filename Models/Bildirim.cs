namespace kutuphane_sistemi.Models;

public class Bildirim
{
    public int BildirimID { get; set; }
    public string Mesaj { get; set; } = string.Empty;
    public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    public bool Okundu { get; set; } = false;
}