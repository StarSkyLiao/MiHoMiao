namespace MiHoMiao.Core.Serialization.Codec.Internal;

public interface IComplexCodec<TSelf, T1, T1Value, T2, T2Value, T3, T3Value> : IBaseCodec<TSelf>
    where TSelf : IComplexCodec<TSelf, T1, T1Value, T2, T2Value, T3, T3Value>
    where T1 : IBaseCodec<T1Value>
    where T2 : IBaseCodec<T2Value>
    where T3 : IBaseCodec<T3Value>
{
    static abstract TSelf CreateSelf(T1Value param1, T2Value param2, T3Value param3);
    
    static abstract T1Value LoadT1(TSelf self);
    
    static abstract T2Value LoadT2(TSelf self);
    
    static abstract T3Value LoadT3(TSelf self);
    
    static TSelf IBaseCodec<TSelf>.Decode(CodecStream buffer)
    {
        T1Value t1 = T1.Decode(buffer);
        T2Value t2 = T2.Decode(buffer);
        T3Value t3 = T3.Decode(buffer);
        return TSelf.CreateSelf(t1, t2, t3);
    }
    
    static void IBaseCodec<TSelf>.Encode(CodecStream buffer, TSelf self)
    {
        T1.Encode(buffer, TSelf.LoadT1(self));
        T2.Encode(buffer, TSelf.LoadT2(self));
        T3.Encode(buffer, TSelf.LoadT3(self));
    }
    
}