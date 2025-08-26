using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxin.Syntax.Grammar;
using MiHoMiao.Migxin.Syntax.Lexical;

namespace MiHoMiao.Migxin;

public class GrammarTest
{
    public const string Input =
        """
        2 + 3 * 4 + a . a
        """;
   
    public static void Run()
    {
        MigxinLexer lexer = MigxinLexer.Parse(Input);
        // Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        // Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));
        
        MigxinGrammar grammar = MigxinGrammar.Parse(lexer);
        Console.WriteLine(grammar.MigxinTrees.GenericViewer("", "", ""));
        Console.WriteLine(grammar.Exceptions.GenericViewer("", "", "\n"));
    }
}