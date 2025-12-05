using Antlr4.Runtime;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;

namespace MiHoMiao.Migxn;

public static class Runner
{
    private const string Input =
        """
        import System.Collection.Generic
        import ArrayList from System.Collection
        import System.Collection.ArrayList as list
        
        asmbly secured List<T> : System.Collections.Generic with IList<T>
        
        native val m_InmultableValue : int;
        native var m_FloatVariable : double = 1.5
        native val m_ConstValue : string = "12345"
        
        asmbly get InmultableValue : int -> 1
        asmbly get MultableVariable : int -> m_MultableVariable
        asmbly set MultableVariable : int -> m_MultableVariable = value
        
        asmbly let NormalProperty : double { native set } = 1.5
        asmbly ref RefProperty : double { set } = m_FloatVariable
        
        asmbly fun Mul() -> 1
        asmbly fun Mul(a : i32, var b : f64) -> 1
        
        as pure virtual newslot concept secured inline static:
        asmbly fun Mul(var a : i32, var b : f64) -> {
            get item -> Math.Pow(2, 3)
            {} |> Pow
            {1} |> Pow
            { { 2, 3 } |> Mul, 0 } |> Mul
            FloatVariable *= self[0]
            MultableVariable *= a
            { 2, 3 } *= 1
            { FloatVariable, MultableVariable } *= { MultableVariable, FloatVariable }
            when (item)
            :: < 1 -> item = 1 > 2
            :: > 2 -> item = 2
            :: not in [1, 2) and not in [3, 5) -> item = 3
            :: f64 -> item = 4
            :: not f64 and not (string or i64) -> item = 5
            :: in [2, ..)
            end
            when (item)
            :: < 1 -> item = 1 > 2
            :: > 2 ->
                when (item)
                :: < 1 -> item = 
                    item when 
                    :: < 1 -> a 
                    :: in [1, 3) -> 2
                    end
                end 
            end
            var a = item when 
            :: < 1 -> a 
            :: in [1, 3) -> 2
            end
        }
        
        as inline:asmbly fun Mul(var a : i32, var b : f64) -> {
            var func = () : int -> 1
            val item : i32 = 1
            var item : i32
            var item = 1
        }
        
        asmbly val GetEnumerator : Func<f64> = () : IEnumerator -> { ret 1 }
        
        asmbly var GetEnumerator : Func<f64> = () : IEnumerator -> 2
        
        native valType List<T>.Enumerator with IEnumerable<T>
        
        """;
    
    public static void Run()
    {
        TimeTest.RunTest(() =>
        {
            var lexer = new MigxnLexer(new AntlrInputStream(Input));
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
        }, nameof(AntlrInputStream), 20, TimeTest.RunTestOption.Best75 | TimeTest.RunTestOption.Sequence);

    }
    
}