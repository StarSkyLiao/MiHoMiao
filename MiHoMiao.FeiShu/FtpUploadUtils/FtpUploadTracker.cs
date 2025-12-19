namespace MiHoMiao.FeiShu.FtpUploadUtils;

/// <summary>
/// FTP上传进度跟踪器
/// </summary>
public class FtpUploadTracker(string filePath)
{
    private long m_SizeWritten;
    private readonly long m_TotalSize = new FileInfo(filePath).Length;

    public void UpdateProgress(long transferred, string speed)
    {
        m_SizeWritten = transferred;
        ConsoleProgress.ShowProgress(transferred, m_TotalSize, speed);
    }

    public double Percent => m_TotalSize <= 0 ? 1.0 : (double)m_SizeWritten / m_TotalSize;
    
}