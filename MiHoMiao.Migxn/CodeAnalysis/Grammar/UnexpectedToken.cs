using MiHoMiao.Migxn.Syntax;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Migxn.CodeAnalysis.Grammar;

public class UnexpectedToken<T>(MigxnToken token) : BadMigxnTree
{

    public override string Message => $"Unexpected token:{token.Text}. Excepted for {typeof(T).Name}";

    public override List<MigxnNode> MigxnTree { get; } = [token];

}