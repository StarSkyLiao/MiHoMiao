using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Program.Jarfter;

internal static class InterpreterProgram
{
    public static void Run()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();

        TimeTest.RunTest(() => jarfterInterpreter.Run(Input),
            nameof(JarfterInterpreter), 5, 
            TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
        );

        TimeTest.RunTest(() => Fibonacci(100),
            nameof(Fibonacci), 5, 
            TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
        );
    }

    private static void Fibonacci(int input)
    {
        long a = 1;
        long b = 1;
        int index = 2;

        do
        {
            long temp = a + b;
            a = b;
            b = temp;
            index++;
        } while (index < input);

        Console.WriteLine(b);
    }

    private const string Input =
        """
        ${
        var input 100,
        var a 1,
        var b 1,
        var temp 0,
        var index 2,
        #,
        label loop.start,
        #,
        let temp (add @a @b),
        let a @b,
        let b @temp,
        inc index,
        #,
        cmp @index @input,
        jl loop.start,
        #,
        load b,
        print (peek),
        }
        """;
}