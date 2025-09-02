using MiHoMiao.Migxn.CodeGen.Compare;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBrTrue(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"br.true",-12}{labelName}";
    
    public override void OnEmitting(List<MigxnOpCode> codes, int index)
    {
        if (index == 0)
        {
            base.OnEmitting(codes, index);
            return;
        }
        MigxnOpCode last = codes[index - 1];
        last = last switch
        {
            OpCgt => new OpBgt(labelName),
            OpCge => new OpBge(labelName),
            OpClt => new OpBlt(labelName),
            OpCle => new OpBle(labelName),
            OpCeq => new OpBeq(labelName),
            OpCne => new OpBne(labelName),
            _ => last,
        };
        if (codes[index - 1] == last) base.OnEmitting(codes, index);
        else codes[index - 1] = last;
    }
}