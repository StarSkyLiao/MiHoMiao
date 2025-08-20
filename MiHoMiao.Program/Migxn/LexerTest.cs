using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        var num1 = 1
        var num2 = 1
        loop(n)
        {
            var nextNum = num1 + num2
            num1 = num2
            num2 = nextNum
            1 = 1
        }
        """;
   
    public static void Run()
    {
        MigxnLexer lexer = MigxnLexer.Parse(Input);
        // Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        // Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));
        MigxnGrammar grammar = MigxnGrammar.Parse(lexer);
        // Console.WriteLine(grammar.MigxnTrees.GenericViewer("", "", "\n"));
        Console.WriteLine(grammar.Exceptions.GenericViewer(item => item.Message, "", "", "\n"));
        using InterpolatedString ilCode = new InterpolatedString(512);
        using InterpolatedString formatCode = new InterpolatedString(512);
        foreach (MigxnTree migxnTree in grammar.MigxnTrees)
        {
            formatCode.Append(migxnTree.Text);
            formatCode.Append('\n');
            foreach (MigxnOpCode migxnOpCode in migxnTree.AsOpCodes())
            {
                ilCode.Append(migxnOpCode.ToString());
                ilCode.Append('\n');
            }
        }
        Console.Write(formatCode.ToString());
        Console.Write(ilCode.ToString());
    }
}