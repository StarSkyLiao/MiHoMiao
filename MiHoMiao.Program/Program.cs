using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Jarfter.Runtime.Core;

JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();

const string Input =
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

TimeTest.RunTest(() => jarfterInterpreter.Run(Input),
    nameof(JarfterInterpreter), 5, 
    TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
);

TimeTest.RunTest(() => Fibonacci(100),
    nameof(Fibonacci), 5, 
    TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
);


void Fibonacci(int input)
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

// JarfterConsole jarfterConsole = new JarfterConsole();
// while (true)
// {
//     string input = Console.ReadLine()!;
//     if (string.IsNullOrEmpty(input)) break;
//     jarfterConsole.RunDebug(input);
// }
// jarfterConsole.RunDebug("print (peek)");