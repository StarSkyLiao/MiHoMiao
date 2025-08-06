using MiHoMiao.Jarfter.Console;
using MiHoMiao.Jarfter.Runtime.Function.Internal.Call;

namespace MiHoMiao.Program.Jarfter;

internal static class ConsoleProgram
{
    public static void Run()
    {
        CallHelper.Register("sqrt", Method);
        JarfterConsole jarfterConsole = new JarfterConsole();
        while (true)
        {
            string input = Console.ReadLine()!;
            if (string.IsNullOrEmpty(input)) break;
            jarfterConsole.Run(input);
        }
        jarfterConsole.Run("print (peek)");
        return;

        double Method(decimal item)
        {
            return Math.Sqrt((double)item);
        }
    }
}