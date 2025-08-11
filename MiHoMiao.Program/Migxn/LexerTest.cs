using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Program.Migxn;

public static class LexerTest
{
    public const string Input =
        """
        1a
        1.1
        1.a
        var item_char = '2'
        {
            var item_i64 : i64 = 5
            var item_f64 : f64 = item_long
            var item_str = "\'\"13123\n\r"
            var item_char = '1'
        }
        {
            var item_char = '2'
        }
        {
            var item_char = '1'
        }
        """;
    
    public static void Run()
    {
        MigxnLexer lexer = new MigxnLexer(Input);
        Console.WriteLine(lexer.Lex().GenericViewer("", "", "\n"));
        Console.WriteLine(lexer.Exceptions.GenericViewer());
    }
}