using System.Security.Cryptography;
using System.Text;

namespace Backend.Helpers;

public class AES
{
    private readonly string encryptionKey;
    private readonly int keySize;
    private readonly int blockSize;

    public AES(string encryptionKey, int keySize = 256, int blockSize = 128)
    {
        if (encryptionKey.Length != keySize / 8)
            throw new ArgumentException($"Encryption key must be {keySize / 8} bytes long.");

        this.encryptionKey = encryptionKey;
        this.keySize = keySize;
        this.blockSize = blockSize;
    }

    public string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.Mode = CipherMode.CFB;
            aesAlg.BlockSize = blockSize;
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                byte[] ivBytes = aesAlg.IV;
                byte[] encryptedData = msEncrypt.ToArray();

                // Combine IV and encrypted data into a single byte array
                byte[] resultBytes = new byte[ivBytes.Length + encryptedData.Length];
                Buffer.BlockCopy(ivBytes, 0, resultBytes, 0, ivBytes.Length);
                Buffer.BlockCopy(encryptedData, 0, resultBytes, ivBytes.Length, encryptedData.Length);

                return Convert.ToBase64String(resultBytes);
            }
        }
    }

    public string Decrypt(string encryptedText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.Mode = CipherMode.CFB;
            aesAlg.BlockSize = blockSize;

            byte[] inputBytes = Convert.FromBase64String(encryptedText);
            byte[] ivBytes = new byte[aesAlg.BlockSize / 8];
            byte[] encryptedData = new byte[inputBytes.Length - ivBytes.Length];

            Buffer.BlockCopy(inputBytes, 0, ivBytes, 0, ivBytes.Length);
            Buffer.BlockCopy(inputBytes, ivBytes.Length, encryptedData, 0, encryptedData.Length);

            aesAlg.IV = ivBytes;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}