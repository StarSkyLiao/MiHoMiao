using System.Text;
using FluentFTP;

namespace MiHoMiao.FeiShu.FtpUploadUtils;

public partial class FtpServer
{
    /// <summary>
    /// 登录并确保连接
    /// </summary>
    private async Task<AsyncFtpClient> LoginAsync()
    {
        if (m_Client != null) await m_Client.DisposeAsync();
        m_Client = new AsyncFtpClient(host, user, pass, port)
        {
            Encoding = Encoding.UTF8,
            Config = { RetryAttempts = 2, DataConnectionType = FtpDataConnectionType.AutoPassive }
        };
        await m_Client.Connect();
        return m_Client;
    }
    
    /// <summary>
    /// 异步加载当前的 Ftp 客户端.
    /// </summary>
    private async Task<AsyncFtpClient> GetClientAsync()
    {
        if (m_Client is { IsConnected: true }) return m_Client;
        return await LoginAsync();
    }
    
    /// <summary>
    /// 检察远端指定位置的文件是否存在
    /// </summary>
    public async Task<bool> FileExistAsync(string fullPath)
    {
        AsyncFtpClient client = await GetClientAsync();
        return await client.FileExists(fullPath);
    }
    
    /// <summary>
    /// 在远端创建指定的文件目录
    /// </summary>
    public async Task<bool> CreateDirectoryAsync(string path)
    {
        AsyncFtpClient client = await GetClientAsync();
        return await client.CreateDirectory(path);
    }
    
    /// <summary>
    /// 上传指定的 localPath 文件到远端的 remotePath 路径
    /// </summary>
    public async Task UploadFileAsync(string localPath, string remoteDir)
    {
        AsyncFtpClient client = await GetClientAsync();
        string remotePath = $"{remoteDir.Replace('\\', '/')}/{Path.GetFileName(localPath)}".Replace("//", "/");
        // 进度回调
        FtpUploadTracker tracker = new FtpUploadTracker(localPath);
        IProgress<FtpProgress> progress = new Progress<FtpProgress>(item =>
            tracker.UpdateProgress(item.TransferredBytes, item.TransferSpeedToString())
        );
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 上传{localPath}到: {remotePath}");
        FtpStatus status = await client.UploadFile(localPath, remotePath, FtpRemoteExists.Overwrite, true, progress: progress);
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 传输完成, 结果为: {status}");
    }
    
    /// <summary>
    /// 上传指定的 localPath 文件到远端的 remotePath 路径
    /// </summary>
    public async Task UploadBytesAsync(byte[] bytes, string remotePath, FtpRemoteExists existsMode, IProgress<FtpProgress> progress)
    {
        AsyncFtpClient client = await GetClientAsync();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 上传 {bytes.Length}Bytes 到: {remotePath}");
        FtpStatus status = await client.UploadBytes(bytes, remotePath, existsMode, true, progress: progress);
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 传输完成, 结果为: {status}");
    }
}