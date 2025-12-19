using FluentFTP;

namespace MiHoMiao.FeiShu.FtpUploadUtils;

/// <summary>
/// FTP工具类
/// </summary>
public sealed partial class FtpServer(string host, int port, string user, string pass) : IAsyncDisposable
{
    private AsyncFtpClient? m_Client;
    
    public async ValueTask DisposeAsync()
    {
        if (m_Client != null) await m_Client.DisposeAsync();
    }
}