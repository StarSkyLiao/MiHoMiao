using MiHoMiao.Migxn.Syntax;

namespace MiHoMiao.Migxn.CodeAnalysis;

public abstract class BadMigxnTree : Exception
{
    public abstract List<MigxnNode> MigxnTree { get; }
}