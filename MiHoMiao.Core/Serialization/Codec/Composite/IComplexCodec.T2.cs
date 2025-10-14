namespace MiHoMiao.Core.Serialization.Codec.Composite;

public interface IComplexCodec<TBuffer, TSelf, T1, T1Value, T2, T2Value> : IBaseCodec<TBuffer, TSelf>
    where TBuffer : allows ref struct
    where TSelf : IComplexCodec<TBuffer, TSelf, T1, T1Value, T2, T2Value>
    where T1 : IBaseCodec<TBuffer, T1Value>
    where T2 : IBaseCodec<TBuffer, T2Value>
{
    static abstract TSelf CreateSelf(T1Value param1, T2Value param2);
    
    static abstract T1Value LoadT1(TSelf self);
    
    static abstract T2Value LoadT2(TSelf self);
    
    static TSelf IBaseCodec<TBuffer, TSelf>.Decode(ref TBuffer buffer)
    {
        T1Value t1 = T1.Decode(ref buffer);
        T2Value t2 = T2.Decode(ref buffer);
        return TSelf.CreateSelf(t1, t2);
    }
    
    static void IBaseCodec<TBuffer, TSelf>.Encode(ref TBuffer buffer, TSelf self)
    {
        T1.Encode(ref buffer, TSelf.LoadT1(self));
        T2.Encode(ref buffer, TSelf.LoadT2(self));
    }
    
}