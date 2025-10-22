namespace MiHoMiao.Core.Serialization.Codec.Internal;

public interface IComplexCodec<TSelf, T1, T1Value> : IBaseCodec<TSelf>
    where TSelf : IComplexCodec<TSelf, T1, T1Value>
    where T1 : IBaseCodec<T1Value>
{
    static abstract TSelf CreateSelf(T1Value param1);
    
    static abstract T1Value LoadT1(TSelf self);
    
    static TSelf IBaseCodec<TSelf>.Decode(CodecStream buffer)
    {
        T1Value t1 = T1.Decode(buffer);
        return TSelf.CreateSelf(t1);
    }
    
    static void IBaseCodec<TSelf>.Encode(CodecStream buffer, TSelf self)
    {
        T1.Encode(buffer, TSelf.LoadT1(self));
    }
    
}