using Antlr4.Runtime;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Antlr.Visitor;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn;

public static class Runner
{
    public static void Run()
    {
        MigxnMethod method = new MigxnMethod(new MigxnContext(), "", typeof(void));
        MigxnLanguage language = new MigxnLanguage(method);
        
        string expression = "var a = 2 + 3 * 4 + a";
        MigxnLiteral lexer = new MigxnLiteral(CharStreams.fromString(expression));
        MigxnStmt parser = new MigxnStmt(new CommonTokenStream(lexer));
        language.Visit(parser.statement());
        Console.Write(language.MigxnMethod.Codes.GenericViewer("", "\n", "\n"));
        Console.Write(language.MigxnMethod.Context.Exceptions.GenericViewer("", "\n", "\n"));
    }
}