using System.Reflection.Emit;
using MiHoMiao.Migxn.Reflection;
using MiHoMiao.Migxn.Syntax.Parser;

namespace MiHoMiao.Migxn.Runtime;

internal class MigxnContext(MigxnParser parser, MigxnContext? parent = null)
{
    internal MigxnParser MigxnParser => parser;
    
    private MigxnContext? Parent { get; set; } = parent;
    
    internal readonly MigxnSymbols<MigxnVariable> Variables = new MigxnSymbols<MigxnVariable>(parent?.Variables);
    
    internal readonly MigxnSymbols<Label> Labels = new MigxnSymbols<Label>(parent?.Labels);

    internal Type? ParseType(string typeName) => ReflectTool.LoadType(typeName);

}