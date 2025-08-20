using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        var i = 0
        while (i < 10) i = i + 1
        """;
    
    // label start:
    // var item = Console.WriteLine(grammar.MigxnTrees.GenericViewer("", "", "\n"))
    // goto start
   
    public static void Run()
    {
        MigxnLexer lexer = MigxnLexer.Parse(Input);
        // Console.WriteLine(lexer.MigxnTokens.GenericViewer("", "", "\n"));
        // Console.WriteLine(lexer.Exceptions.GenericViewer("", "", "\n"));
        MigxnGrammar grammar = MigxnGrammar.Parse(lexer);
        // Console.WriteLine(grammar.MigxnTrees.GenericViewer("", "", "\n"));
        // Console.WriteLine(grammar.Exceptions.GenericViewer(item => item.Message, "", "", "\n"));
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