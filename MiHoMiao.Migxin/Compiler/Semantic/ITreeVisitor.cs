using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxin.Compiler.Syntax;

namespace MiHoMiao.Migxin.Compiler.Semantic.Stmt;

internal interface ITreeVisitor
{
    /// <summary>
    /// 对应的原始 Token 的类型
    /// </summary>
    public static abstract Type TreeType { get; }

    /// <summary>
    /// 访问指定的语法树.
    /// </summary>
    public static abstract void Visit(MigxinParser migxinParser, MigxinNode node);
    
    public delegate void Visitor(MigxinParser migxinParser, MigxinNode node);
    
    #region Reflection

    [field: AllowNull, MaybeNull]
    private static List<Type> VisitorTypes => field ??=
    [
        ..
        from type in typeof(ITreeVisitor).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(ITreeVisitor)) && !type.IsAbstract && !type.IsInterface
        select type
    ];    
    
    [field: AllowNull, MaybeNull]
    internal static Dictionary<Type, Visitor> TreeVisitors
    {
        get
        {
            if (field != null) return field;

            field = [];
            foreach (Type type in VisitorTypes)
            {
                object? tokenType = type.GetProperty(nameof(TreeType))?.GetValue(null);
                Debug.Assert(tokenType != null);
                var func = type.GetMethod(nameof(Visit))?.CreateDelegate<Visitor>();
                Debug.Assert(func != null);
                field.Add((Type)tokenType, func);
            }

            return field;
        }
    }

    #endregion

}

internal interface ITreeVisitor<TVisitor, in TTree> : ITreeVisitor
    where TVisitor : ITreeVisitor<TVisitor, TTree> where TTree : MigxinTree
{
    
    static Type ITreeVisitor.TreeType => typeof(TTree);

    static void ITreeVisitor.Visit(MigxinParser migxinParser, MigxinNode node) => TVisitor.Visit(migxinParser, (node as TTree)!);
    
    public static abstract void Visit(MigxinParser migxinParser, TTree tree);

}