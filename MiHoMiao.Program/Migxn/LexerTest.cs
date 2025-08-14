using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        (1+(2+3)*4++) . (1)
        """;
    
    public static void Run()
    {
        MigxnLexer lexer = MigxnLexer.Parse(Input);
        // Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        // Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));

        MigxnGrammar grammar = MigxnGrammar.Parse(lexer);
        Console.WriteLine(grammar.MigxnTrees.GenericViewer("", "", "\n"));
        Console.WriteLine(grammar.Exceptions.GenericViewer(item => item.Message, "", "", "\n"));
    }
}