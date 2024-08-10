using System;
using System.Drawing.Text;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public class DPAPI
{
    // DPAPI Example: https://learn.microsoft.com/en-us/dotnet/standard/security/how-to-use-data-protection

    private static byte[] s_additionalEntropy = { 156, 11, 77, 97, 65 };
    public static byte[] ProtectByte(byte[] data)
    {
        try
        {
            return ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
        }
        catch ( CryptographicException ex)
        {
            MessageBox.Show(ex.Message);
            MessageBox.Show(ex.StackTrace);
        }
        return null;
    }
    public static byte[] UnprotectByte(byte[] data)
    {
        try
        {
            return ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
        }
        catch (CryptographicException ex)
        {
            MessageBox.Show(ex.Message);
            MessageBox.Show(ex.StackTrace);
        }
        return null;
    }
}