using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Lexers;
using MiHoMiao.Migxn.Syntax.Parser;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        var num1 : i64
        var num2 = 1
        var nextNum = num1 and (num1 or num2)
        """;
   
    public static void Run()
    {
        MigxnLexer lexer = MigxnLexer.Parse(Input);
        // Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        // Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));
        MigxnGrammar grammar = MigxnGrammar.Parse(lexer);
        // Console.WriteLine(grammar.CodeFormat());
        // Console.WriteLine(grammar.MigxnTrees.GenericViewer("", "", "\n"));
        // Console.WriteLine(grammar.Exceptions.GenericViewer(item => item.Message, "", "", "\n"));
        MigxnParser parser = MigxnParser.Parse(grammar);
        Console.WriteLine(parser.CodeFormat());
        Console.WriteLine(parser.Exceptions.GenericViewer(item => item.Message, "", "", "\n"));
    }
}