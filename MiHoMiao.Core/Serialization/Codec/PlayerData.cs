using SelfCodec = MiHoMiao.Core.Serialization.Codec.Composite.IComplexCodec<
    System.Span<byte>, MiHoMiao.Core.Serialization.Codec.PlayerData, 
    MiHoMiao.Core.Serialization.Codec.Internal.GuidCodec, System.Guid, 
    MiHoMiao.Core.Serialization.Codec.Internal.Int32Codec, int
>;

namespace MiHoMiao.Core.Serialization.Codec;

public record PlayerData(Guid Id, int Level) : SelfCodec
{
    static PlayerData SelfCodec.CreateSelf(Guid param1, int param2) => new PlayerData(param1, param2);

    static Guid SelfCodec.LoadT1(PlayerData self) => self.Id;

    static int SelfCodec.LoadT2(PlayerData self) => self.Level;
}