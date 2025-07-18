namespace MiHoMiao.Core.Serialization.IO;

/// <summary>
/// 提供文件操作的扩展方法，用于简化文件的读写功能。
/// 该类包含将字符串写入文件和从文件中读取字符串的静态方法。
/// </summary>
public static class FileOperation
{
    /// <summary>
    /// 将字符串内容写入指定路径的文件。
    /// 如果目标文件的目录不存在，将自动创建目录。
    /// </summary>
    /// <param name="content">要写入文件的字符串内容。</param>
    /// <param name="path">目标文件的路径。</param>
    public static void ToFile(this string content, string path)
    {
        string? directoryPath = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        File.WriteAllText(path, content);
    }
    
    /// <summary>
    /// 从指定路径的文件读取字符串内容。
    /// 如果文件不存在，返回 null。
    /// </summary>
    /// <param name="path">要读取的文件的路径。</param>
    /// <returns>文件内容的字符串，如果文件不存在则返回 null。</returns>
    public static string? FromFile(this string path) => File.Exists(path) ? File.ReadAllText(path) : null;
    
}