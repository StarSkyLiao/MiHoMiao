namespace MiHoMiao.Migxn.CodeGen;

internal abstract class MigxnOpCode
{
    public abstract override string ToString();

    public virtual void OnEmitting(List<MigxnOpCode> codes, int index) => codes.Insert(index, this);
}