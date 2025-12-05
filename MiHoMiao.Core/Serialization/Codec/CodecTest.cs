// using System.Text.Json;
// using MiHoMiao.Core.Diagnostics;
// using MiHoMiao.Core.Serialization.Json;
//
// namespace MiHoMiao.Core.Serialization.Codec;
//
// internal static class CodecTest
// {
//     private static readonly PlayerGroup s_InData = new PlayerGroup(
//         new PlayerData(Guid.NewGuid(), 99, "666", [1, 2, 3, 4, 5]), 10,
//         ["AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "FFF"], 
//         [["11", "22"], ["33", "44"]]
//     );
//     
//     public static void FuncTest()
//     {
//         CodecStream buf = CodecEncoder.Encode(s_InData);
//         CodecStream decode = new CodecStream(buf.ToString());
//         decode.Decode(out PlayerGroup outData);
//
//         Console.WriteLine($"Original: {s_InData}");
//         Console.WriteLine($"Decoded : {outData}");
//         Console.WriteLine($"Equal   : {s_InData == outData}");
//         Console.WriteLine($"String  : {buf}");
//         Console.WriteLine($"String  : {decode}");
//     }
//     
//     public static void PerfTest()
//     {
//         TimeTest.RunTest(() =>
//         {
//             CodecStream buf = CodecEncoder.Encode(s_InData);
//             CodecStream decode = new CodecStream(buf.ToString());
//             decode.Decode(out PlayerGroup outData);
//         }, nameof(CodecEncoder.Encode), 24, TimeTest.RunTestOption.Sequence | TimeTest.RunTestOption.Warm);
//     }
//     
//     public static void CompareToJson()
//     {
//         TimeTest.RunTest(() =>
//         {
//             CodecStream _ = CodecEncoder.Encode(s_InData);
//         }, nameof(CodecEncoder.Encode), 24, TimeTest.RunTestOption.Sequence | TimeTest.RunTestOption.Warm);
//         
//         TimeTest.RunTest(() =>
//         {
//             string _ = s_InData.ToJson();
//         }, nameof(JsonSerializer.Serialize), 16, TimeTest.RunTestOption.Sequence | TimeTest.RunTestOption.Warm);
//         
//         CodecStream buf = CodecEncoder.Encode(s_InData);
//         TimeTest.RunTest(() =>
//         {
//             CodecStream decode = new CodecStream(buf.ToString());
//             PlayerGroup _ = decode.Decode<PlayerGroup>();
//         }, nameof(CodecEncoder.Decode), 16, TimeTest.RunTestOption.Sequence | TimeTest.RunTestOption.Warm);
//         
//         string json = s_InData.ToJson();
//         TimeTest.RunTest(() =>
//         {
//             PlayerGroup _ = json.FromJson<PlayerGroup>()!;
//         }, nameof(JsonSerializer.Deserialize), 16, TimeTest.RunTestOption.Sequence | TimeTest.RunTestOption.Warm);
//         
//     }
// }