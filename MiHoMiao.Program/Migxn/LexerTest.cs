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
        if(bool_1) {
        var a = 400
        var b = 1
        }
        else if (bool_1) var a = 6
        else var a = 4
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