using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.Syntax.Parser;

internal class MigxnParser
{
    private MigxnParser(MigxnGrammar grammar)
    {
        Exceptions = [..grammar.Exceptions];
    }
    
    public static MigxnParser Parse(MigxnGrammar grammar)
    {
        MigxnParser result = new MigxnParser(grammar);
        return result;
    }
    
    /// <summary>
    /// 存储解析过程中的异常
    /// </summary>
    internal readonly List<Exception> Exceptions;
}