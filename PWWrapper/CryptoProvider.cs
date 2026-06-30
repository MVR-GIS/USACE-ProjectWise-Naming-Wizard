using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptoProvider
{
    public static string DecryptData(string sEncrpytedData, string sKey)
    {
        byte[] buffer = Convert.FromBase64String(sEncrpytedData);
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        provider.Key = TruncateHash(sKey, provider.KeySize / 8);
        provider.IV = TruncateHash("", provider.BlockSize / 8);
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
        stream2.Write(buffer, 0, buffer.Length);
        stream2.FlushFinalBlock();
        return Encoding.Unicode.GetString(stream.ToArray());
    }

    public static string EncryptData(string sData, string sKey)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(sData);
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        provider.Key = TruncateHash(sKey, provider.KeySize / 8);
        provider.IV = TruncateHash("", provider.BlockSize / 8);
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
        stream2.Write(bytes, 0, bytes.Length);
        stream2.FlushFinalBlock();
        return Convert.ToBase64String(stream.ToArray());
    }

    private static string EncryptDecrypt(string sDatasource, string sUserName, string sPassword, bool bEncrypt)
    {
        if ((!string.IsNullOrEmpty(sDatasource) && !string.IsNullOrEmpty(sUserName)) && !string.IsNullOrEmpty(sPassword))
        {
            string sKey = string.Format("{0}_{1}", sDatasource.ToLower(), sUserName.ToLower());
            try
            {
                if (bEncrypt)
                {
                    return EncryptData(sPassword, sKey);
                }
                return DecryptData(sPassword, sKey);
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("Error: {0}", exception.Message));
                Console.WriteLine(string.Format("Details: {0}", exception.StackTrace));
            }
        }
        return string.Empty;
    }

    public static string GetDecryptedPassword(string sDataSource, string sUserName, string sPassword)
    {
        return EncryptDecrypt(sDataSource, sUserName, sPassword, false);
    }

    public static string GetEncryptedPassword(string sDataSource, string sUserName, string sPassword)
    {
        return EncryptDecrypt(sDataSource, sUserName, sPassword, true);
    }

    private static byte[] TruncateHash(string sKey, int iLength)
    {
        SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
        byte[] bytes = Encoding.Unicode.GetBytes(sKey);
        byte[] buffer2 = provider.ComputeHash(bytes);
        byte[] buffer3 = new byte[iLength];
        for (int i = 0; i < buffer3.Length; i++)
        {
            if (i < (buffer2.Length - 1))
            {
                buffer3[i] = buffer2[i];
            }
            else
            {
                buffer3[i] = 0;
            }
        }
        return buffer3;
    }
}
