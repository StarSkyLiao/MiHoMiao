namespace MiHoMiao.Migxn.Runtime.Variable;

internal abstract record MigxnVariable(string Name, Type Type)
{

    /// <summary>
    /// 该变量是否可写
    /// </summary>
    public bool IsWritable { get; set; }

}