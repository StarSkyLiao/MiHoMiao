using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Parser;

internal class MigxnParser
{
    private MigxnParser(MigxnGrammar grammar)
    {
        Exceptions = [..grammar.Exceptions];
        MigxnOpCodes = [..ParseCodes(grammar)];
    }
    
    public static MigxnParser Parse(MigxnGrammar grammar) => new MigxnParser(grammar);
    
    /// <summary>
    /// 存储解析过程中的异常
    /// </summary>
    internal List<Exception> Exceptions { get; }

    internal List<MigxnOpCode> MigxnOpCodes { get; }

    private IEnumerable<MigxnOpCode> ParseCodes(MigxnGrammar grammar)
    {
        MigxnContext context = new MigxnContext(this);
        foreach (MigxnTree migxnTree in grammar.MigxnTrees)
        {
            foreach (MigxnOpCode migxnOpCode in migxnTree.AsOpCodes(context))
            {
                yield return migxnOpCode;
            }
        }
    }
    
    public string CodeFormat()
    {
        using InterpolatedString formatCode = new InterpolatedString(512);
        foreach (MigxnOpCode migxnCode in MigxnOpCodes)
        {
            formatCode.Append(migxnCode.ToString());
            formatCode.Append('\n');
        }

        return formatCode.ToString();
    }
    
    
}