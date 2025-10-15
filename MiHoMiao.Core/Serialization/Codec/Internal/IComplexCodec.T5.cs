namespace MiHoMiao.Core.Serialization.Codec.Internal;

public interface IComplexCodec<TSelf, T1, T1Value, T2, T2Value, T3, T3Value, T4, T4Value, T5, T5Value>
    : IBaseCodec<TSelf>
    where TSelf : IComplexCodec<TSelf, T1, T1Value, T2, T2Value, T3, T3Value, T4, T4Value, T5, T5Value>
    where T1 : IBaseCodec<T1Value>
    where T2 : IBaseCodec<T2Value>
    where T3 : IBaseCodec<T3Value>
    where T4 : IBaseCodec<T4Value>
    where T5 : IBaseCodec<T5Value>
{
    static abstract TSelf CreateSelf(T1Value p1, T2Value p2, T3Value p3, T4Value p4, T5Value p5);
    static abstract T1Value LoadT1(TSelf self);
    static abstract T2Value LoadT2(TSelf self);
    static abstract T3Value LoadT3(TSelf self);
    static abstract T4Value LoadT4(TSelf self);
    static abstract T5Value LoadT5(TSelf self);

    static TSelf IBaseCodec<TSelf>.Decode(CodecStream buffer)
    {
        T1Value v1 = T1.Decode(buffer);
        T2Value v2 = T2.Decode(buffer);
        T3Value v3 = T3.Decode(buffer);
        T4Value v4 = T4.Decode(buffer);
        T5Value v5 = T5.Decode(buffer);
        return TSelf.CreateSelf(v1, v2, v3, v4, v5);
    }
    static void IBaseCodec<TSelf>.Encode(CodecStream buffer, TSelf self)
    {
        T1.Encode(buffer, TSelf.LoadT1(self));
        T2.Encode(buffer, TSelf.LoadT2(self));
        T3.Encode(buffer, TSelf.LoadT3(self));
        T4.Encode(buffer, TSelf.LoadT4(self));
        T5.Encode(buffer, TSelf.LoadT5(self));
    }
}