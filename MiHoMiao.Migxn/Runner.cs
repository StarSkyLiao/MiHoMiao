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
            var a : int32 = 1
            if (1) a = 2
            else a = 3
            loop (8) a = a + 1
        }
        """;
    
    public static void Run()
    {
        MigxnMethod method = new MigxnMethod(new MigxnContext(), "", typeof(void));
        MigxnCommonParser commonParser = new MigxnCommonParser(method);
        
        MigxnLiteral lexer = new MigxnLiteral(CharStreams.fromString(Input));
        MigxnLanguage parser = new MigxnLanguage(new CommonTokenStream(lexer));
        commonParser.Visit(parser.statement());
        Console.Write(commonParser.Codes.GenericViewer("", "\n", "\n"));
        Console.Write(commonParser.Exceptions.GenericViewer("", "\n", "\n"));
    }
}