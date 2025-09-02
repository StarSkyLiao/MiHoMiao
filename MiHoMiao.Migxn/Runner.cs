using Antlr4.Runtime;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Antlr;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn;

public static class Runner
{
    private const string Input =
        """
        fun Foo() : void -> {
            var a : r64 = 1
            a = 1 * a + 3
            while(a) {
                if (a) ret a
                a = a + 1
            }
        }
        """;
    
    public static void Run()
    {
        MigxnContext migxnContext = new MigxnContext();
        // MigxnMethod method = new MigxnMethod(migxnContext, "", typeof(void));
        MigxnMemberParser methodParser = new MigxnMemberParser(migxnContext);
        
        MigxnLiteral lexer = new MigxnLiteral(CharStreams.fromString(Input));
        MigxnLanguage parser = new MigxnLanguage(new CommonTokenStream(lexer));
        
        methodParser.Visit(parser.language());
        
        Console.Write(migxnContext.AllMembers.GenericViewer("", "\n", "\n"));
        Console.Write(migxnContext.Exceptions.GenericViewer("", "\n", "\n"));
        
    }
}