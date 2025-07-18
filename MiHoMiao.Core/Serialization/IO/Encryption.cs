using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace MiHoMiao.Core.Serialization.IO;

/// <summary>
/// 提供字符串加密、解密、压缩、解压缩以及 Base64 编码解码的扩展方法。
/// 该类包含一系列静态方法，用于对字符串进行安全处理，包括 AES 加密、GZip 压缩和 Base64 转换，适用于数据传输和存储场景。
/// </summary>
public static class Encryption
{
    /// <summary>
    /// 32 字节的 AES 加密密钥，用于加密和解密操作。
    /// </summary>
    private const string Key = "INuSE2Wd0nmEsN6lkXBcJSx76yDEJ1pl";

    /// <summary>
    /// 16 字节的初始化向量 (IV)，用于 AES 加密的 CBC 模式。
    /// </summary>
    private const string Iv = "b1qTpmZsg68azynB";

    /// <summary>
    /// 将字符串转换为 Base64 编码字符串。
    /// 使用 UTF-8 编码将输入字符串转换为字节数组，然后进行 Base64 编码。
    /// </summary>
    /// <param name="rawString">要编码的原始字符串。</param>
    /// <returns>Base64 编码后的字符串。</returns>
    public static string ToBase64(this string rawString)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(rawString));

    /// <summary>
    /// 将 Base64 编码的字符串解码为原始字符串。
    /// 将 Base64 字符串转换为字节数组，然后使用 UTF-8 编码解码为字符串。
    /// </summary>
    /// <param name="base64String">要解码的 Base64 字符串。</param>
    /// <returns>解码后的原始字符串。</returns>
    public static string FromBase64(this string base64String)
        => Encoding.UTF8.GetString(Convert.FromBase64String(base64String));

    /// <summary>
    /// 使用 GZip 算法压缩字符串。
    /// 将输入字符串转换为 UTF-8 编码的字节数组，压缩后转换为 Base64 字符串以便于存储或传输。
    /// </summary>
    /// <param name="str">要压缩的字符串。</param>
    /// <returns>压缩后的 Base64 编码字符串。如果输入为空或 null，则返回原字符串。</returns>
    public static string Compress(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        byte[] bytes = Encoding.UTF8.GetBytes(str);
        using MemoryStream memoryStream = new MemoryStream();
        using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
        {
            gZipStream.Write(bytes, 0, bytes.Length);
        }
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    /// <summary>
    /// 解压缩 GZip 压缩的 Base64 编码字符串。
    /// 将 Base64 字符串解码为字节数组，使用 GZip 解压缩后转换为 UTF-8 编码的字符串。
    /// </summary>
    /// <param name="compressedString">压缩的 Base64 编码字符串。</param>
    /// <returns>解压缩后的原始字符串。如果输入为空或 null，则返回原字符串。</returns>
    public static string Decompress(this string compressedString)
    {
        if (string.IsNullOrEmpty(compressedString)) return compressedString;

        byte[] compressedBytes = Convert.FromBase64String(compressedString);
        using MemoryStream msi = new MemoryStream(compressedBytes);
        using MemoryStream memoryStream = new MemoryStream();
        using (GZipStream gZipStream = new GZipStream(msi, CompressionMode.Decompress))
        {
            gZipStream.CopyTo(memoryStream);
        }
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    /// <summary>
    /// 先压缩字符串，然后进行 AES 加密。
    /// 将输入字符串先通过 GZip 压缩，再使用 AES 算法加密为 Base64 编码字符串。
    /// </summary>
    /// <param name="plainText">要压缩和加密的字符串。</param>
    /// <returns>压缩并加密后的 Base64 编码字符串。</returns>
    public static string EncryptCompress(this string plainText)
        => plainText.Compress().Encrypt();

    /// <summary>
    /// 先解密字符串，然后进行 GZip 解压缩。
    /// 将输入的 Base64 编码字符串先使用 AES 解密，再通过 GZip 解压缩为原始字符串。
    /// </summary>
    /// <param name="plainText">要解密和解压缩的 Base64 编码字符串。</param>
    /// <returns>解密并解压缩后的原始字符串。</returns>
    public static string DecryptDecompress(this string plainText)
        => plainText.Decrypt().Decompress();

    /// <summary>
    /// 使用 AES 算法加密字符串。
    /// 使用 CBC 模式和 PKCS7 填充，将输入字符串加密为 Base64 编码字符串。
    /// </summary>
    /// <param name="plainText">要加密的字符串。</param>
    /// <returns>加密后的 Base64 编码字符串。如果输入为空或 null，则返回原字符串。</returns>
    public static string Encrypt(this string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = Encoding.UTF8.GetBytes(Iv);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            using StreamWriter sw = new StreamWriter(cs);
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// 使用 AES 算法解密字符串。
    /// 将 Base64 编码的加密字符串解密为原始字符串，使用 CBC 模式和 PKCS7 填充。
    /// </summary>
    /// <param name="cipherText">要解密的 Base64 编码字符串。</param>
    /// <returns>解密后的原始字符串。如果输入为空或 null，则返回原字符串。</returns>
    public static string Decrypt(this string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        byte[] buffer = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = Encoding.UTF8.GetBytes(Iv);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream ms = new MemoryStream(buffer);
        using CryptoStream cs = new CryptoStream(
            ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read
        );
        using StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}