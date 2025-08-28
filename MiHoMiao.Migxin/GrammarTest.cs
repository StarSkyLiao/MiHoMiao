using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxin.Compiler.Lexical;
using MiHoMiao.Migxin.Compiler.Syntax;

namespace MiHoMiao.Migxin;

public class GrammarTest
{
    public const string Input =
        """
        var a = a.a! + ++a.a + ++ 3!!
        var b var c : i32 1 + 1 a = 1
        """;
   
    public static void Run()
    {
        MigxinLexer lexer = MigxinLexer.Parse(Input);
        // Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        // Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));
        
        MigxinGrammar grammar = MigxinGrammar.Parse(lexer);
        Console.WriteLine(grammar.CodeFormat());
        Console.WriteLine(grammar.MigxinNodes.GenericViewer("", "", ""));
        Console.WriteLine(grammar.Exceptions.GenericViewer("", "", "\n"));
    }
}