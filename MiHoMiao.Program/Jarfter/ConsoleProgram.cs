using MiHoMiao.Jarfter.Console;

namespace MiHoMiao.Program.Jarfter;

internal static class ConsoleProgram
{
    public static void Run()
    {
        JarfterConsole jarfterConsole = new JarfterConsole();
        while (true)
        {
            string input = Console.ReadLine()!;
            if (string.IsNullOrEmpty(input)) break;
            jarfterConsole.RunDebug(input);
        }
        jarfterConsole.RunDebug("print (peek)");
    }
}