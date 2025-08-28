using MiHoMiao.Migxin.Compiler.Lexical;
using MiHoMiao.Migxin.Compiler.Semantic;
using MiHoMiao.Migxin.Compiler.Syntax;

namespace MiHoMiao.Migxin;

public class ParserTest
{
    public const string Input =
        """
        var a = a.a! + ++a.a + ++ 3!
        var b var c : i32 1 + 1 a = 1
        """;
   
    public static void Run()
    {
        MigxinLexer lexer = MigxinLexer.Parse(Input);
        MigxinGrammar grammar = MigxinGrammar.Parse(lexer);
        MigxinParser parser = MigxinParser.Parse(grammar);

    }
}