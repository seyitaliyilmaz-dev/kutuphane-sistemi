using System.Security.Cryptography;
using System.Text;

namespace kutuphane_sistemi;

public static class SifreYardimcisi
{
    public static string Hashle(string sifre)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(sifre));
        return Convert.ToBase64String(bytes);
    }

    public static bool Dogrula(string girilenSifre, string hashlenmisSifre)
    {
        return Hashle(girilenSifre) == hashlenmisSifre;
    }
}