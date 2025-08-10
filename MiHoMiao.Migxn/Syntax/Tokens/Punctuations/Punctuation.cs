using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public abstract record Punctuation(int Position, ReadOnlyMemory<char> Text) : MigxnToken(Position, Text)
{
    internal override void EmitCode(MigxnContext context, ILGenerator generator) => throw new NotSupportedException();

    public static Punctuation Create(int position, ReadOnlySpan<char> input) => input switch
    {
        "+" => new AddToken(position),
        "-" => new SubToken(position),
        "*" => new MulToken(position),
        "/" => new DivToken(position),
        "%" => new ModToken(position),
        "=" => new EqlToken(position),
        "!" => new NotToken(position),
        "." => new FldToken(position),
        ":" => new ColonToken(position),
        "!=" => new UeqToken(position),
        "(" => new RoundRightToken(position),
        ")" => new RoundLeftToken(position),
        "{" => new CurlyLeftToken(position),
        "}" => new CurlyRightToken(position),
        _ => throw new ArgumentOutOfRangeException(input.ToString())
    };
    
}