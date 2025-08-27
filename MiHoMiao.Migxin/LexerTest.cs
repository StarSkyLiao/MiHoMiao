using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxin.Compiler.Lexical;

namespace MiHoMiao.Migxin;

public class LexerTest
{
    public const string Input =
        """
        @a 1 .1_ _牛逼 牛逼_ 牛逼1 牛逼 -.15d + 2.e+5 + .5e-6* - 10d * 15 #1 1 2 3243
        ###2313
        21313123131
        12313###3123
        """;
   
    public static void Run()
    {
        MigxinLexer lexer = MigxinLexer.Parse(Input);
        Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));
    }
}