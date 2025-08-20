using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        if(bool_1) {
        a = 4
        b = 1
        }
        else if (bool_1) a = 6
        else a = 4
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
        foreach (var migxnTree in grammar.MigxnTrees)
        {
            foreach (MigxnOpCode migxnOpCode in migxnTree.AsOpCodes())
            {
                Console.WriteLine(migxnOpCode);
            }
        }
    }
}