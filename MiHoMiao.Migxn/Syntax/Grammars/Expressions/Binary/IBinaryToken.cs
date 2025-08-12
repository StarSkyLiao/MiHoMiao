namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

public interface IBinaryToken
{
    /// <summary>
    /// 优先级越小, 实际优先级越高
    /// </summary>
    abstract int Priority { get; }
}