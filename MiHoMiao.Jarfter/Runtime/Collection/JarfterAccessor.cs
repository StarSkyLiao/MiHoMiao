using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Collection;

public class JarfterAccessor(JarfterObject content, string name) : JarfterObject, IJarfterParsable<JarfterAccessor>
{
    public JarfterObject Content => content;
    
    public string VarName => name;

    public override string ToString() => $"@{name}";

    private class TestSymbolTable : JarfterSymbolTable<JarfterObject>
    {
        public override JarfterObject? LoadVariable(string name) => new JarfterObject();
    }
    
    internal static JarfterAccessor ParseInternal(ReadOnlySpan<char> input)
    {
        JarfterContext jarfterContext = new JarfterContext(null!)
        {
            JarfterSymbolTable = new TestSymbolTable()
        };
        return Parse(input, jarfterContext);
    }

    public new static JarfterAccessor Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
    {
        if (input.Length == 0) throw new InvalidTypeException<JarfterAccessor>("Empty");
        if (provider is not JarfterContext context) throw new InvalidCallingTreeException();
        ref int index = ref context.ParsingIndex;
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        if (IsPunctuation(input[index]) || char.IsWhiteSpace(input[index]))
            throw new InvalidTypeException<JarfterAccessor>(input[index].ToString());
        string varName = JarfterWord.ParseSpan(input, context).ToString();
        return new JarfterAccessor(context.JarfterSymbolTable.LoadVariable(varName)!, varName);
    }
    
    private static bool IsPunctuation(char input) => char.IsPunctuation(input) && (input is not '.' and not '_');
    
}