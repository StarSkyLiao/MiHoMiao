using Antlr4.Runtime;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Antlr.Visitor;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn;

public static class Runner
{
    private const string Input =
        """
        {
            {
                var a = 2 + 3 * 4 + 1
                a = 12 + a
            }
            {
                val a = 2 + 3 * 4 + 1
                a = 12 + a
            }
        }
        """;
    
    public static void Run()
    {
        MigxnMethod method = new MigxnMethod(new MigxnContext(), "", typeof(void));
        MigxnLanguage language = new MigxnLanguage(method);
        
        MigxnLiteral lexer = new MigxnLiteral(CharStreams.fromString(Input));
        MigxnStmt parser = new MigxnStmt(new CommonTokenStream(lexer));
        language.Visit(parser.statement());
        Console.Write(language.Codes.GenericViewer("", "\n", "\n"));
        Console.Write(language.Exceptions.GenericViewer("", "\n", "\n"));
    }
}