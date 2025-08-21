using MiHoMiao.Migxn.Syntax;

namespace MiHoMiao.Migxn.CodeAnalysis;

internal abstract class BadMigxnTree : Exception
{
    public abstract List<MigxnNode> MigxnTree { get; }
}