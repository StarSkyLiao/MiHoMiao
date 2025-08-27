using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxin.Compiler.Lexical;

namespace MiHoMiao.Migxin.Compiler.Syntax.Stmt;

internal abstract record MigxinStmt(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinTree(Text, Index, Position)
{

    #region Reflection

    [field: AllowNull, MaybeNull]
    private static List<Type> StmtTypes => field ??=
    [
        ..
        from type in typeof(MigxinToken).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(ITokenGrammar)) && !type.IsAbstract && !type.IsInterface
        select type
    ];    
    
    [field: AllowNull, MaybeNull]
    internal static Dictionary<Type, ITokenGrammar.StmtParser> StmtParsers
    {
        get
        {
            if (field != null) return field;

            field = [];
            foreach (Type type in StmtTypes)
            {
                object? tokenType = type.GetProperty(nameof(ITokenGrammar.TokenType))?.GetValue(null);
                Debug.Assert(tokenType != null);
                var func = type.GetMethod(nameof(ITokenGrammar.TryMatchStmt))?.CreateDelegate<ITokenGrammar.StmtParser>();
                Debug.Assert(func != null);
                field.Add((Type)tokenType, func);
            }

            return field;
        }
    }

    #endregion
    
}