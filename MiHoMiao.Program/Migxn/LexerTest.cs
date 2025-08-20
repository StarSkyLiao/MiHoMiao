using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        if(1) a = 1
        var a = 0
        var b = 1
        var i = 0
        label start:
          goto end
          var temp = a
          a = b
          b = temp + a
          i = i + 1
          goto start
        label end:
        
        
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