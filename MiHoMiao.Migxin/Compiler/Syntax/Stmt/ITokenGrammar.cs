namespace MiHoMiao.Migxin.Compiler.Syntax.Stmt;

internal interface ITokenGrammar
{
    /// <summary>
    /// 对应的原始 Token 的类型
    /// </summary>
    public static abstract Type TokenType { get; }

    /// <summary>
    /// 生成指定的 语句体.
    /// </summary>
    public static abstract MigxinResult<MigxinStmt> TryMatchStmt(MigxinGrammar migxinGrammar);
    
    public delegate MigxinResult<MigxinStmt> StmtParser(MigxinGrammar migxinGrammar);
    
}