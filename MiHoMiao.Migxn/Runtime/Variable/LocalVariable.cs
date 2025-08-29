namespace MiHoMiao.Migxn.Runtime.Variable;

internal record LocalVariable(string Name, Type Type) : MigxnVariable(Name, Type)
{
    
}