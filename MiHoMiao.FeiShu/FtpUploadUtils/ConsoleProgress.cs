namespace MiHoMiao.FeiShu.FtpUploadUtils;

/// <summary>
/// 控制台进度显示
/// </summary>
public static class ConsoleProgress
{
    public static void ShowProgress(long transferred, long total, string speed)
    {
        if (total <= 0) return;
        double percent = (double)transferred / total;
        const int BarWidth = 50;
        int pos = (int)(BarWidth * percent);
        Console.WriteLine($"[{speed,12}][{new string('=', pos),-BarWidth}] {transferred}Bytes/{total}Bytes :{percent:P1}");
    }
}