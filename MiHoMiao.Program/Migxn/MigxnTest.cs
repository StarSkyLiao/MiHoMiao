namespace MiHoMiao.Program.Migxn;

public class MigxnTest
{
    public const string Input =
        """
        var item_char = '2'
        {
            var item_long : i64 = 5
            var item_double : r64 = item_long
            var item_string = "\'\"13123\n\r"
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
        // MigxnAssembly assembly = new MigxnAssembly("Migxn");
        // MigxnModule module = assembly.CreateModule("MyModule");
        //
        // IEnumerable<MigxnToken> tokens = new MigxnLexer().Parse(Input);
        // MigxnTree? item = new MigxnParser().Parse(tokens);
        // Debug.Assert(item != null);
        //
        // MethodInfo method = module.CompileMethod("TestMethod", item);
        //
        // module.Build();
        // assembly.Save();
        // assembly.PrintMethodInfo(module, "TestMethod");

    }

    public static void Method()
    {
        {
            var item_long = 5;
            double item_double = item_long;
            var item_string = "\'\"13123\n\r";
            var item_char = '1';
            Console.Write(item_string + item_long + item_double + item_char);
            Console.Write(item_string + item_long + item_double + item_char);
        }
        {
            var item_char = '1';
            Console.Write(item_char);
        }
    }
}