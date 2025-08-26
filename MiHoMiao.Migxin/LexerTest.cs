using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxin.Syntax.Lexical;

namespace MiHoMiao.Migxin;

public class LexerTest
{
    public const string Input =
        """
        1 + 2 + 5 - 10 * 15 #1 1 2 3243
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