// using System.Globalization;
// using MiHoMiao.Jarfter.Core.Collection;
//
// namespace MiHoMiao.xUnit.Jarfter;
//
// public class CollectionTests
// {
//     [Theory]
//     [InlineData("[1,    2   , 3]", new[] { 1, 2, 3 })]
//     [InlineData("[ 42 ]", new[] { 42 })]
//     [InlineData("[ ]", new int[0])]
//     [InlineData("[1,   2,  3,   ]", new[] { 1, 2, 3 })] // 末尾逗号容忍
//     public void Array_TryParse_Int(string input, int[] expected)
//     {
//         bool success = JarfterArray<int>.TryParse(input, CultureInfo.InvariantCulture, out JarfterArray<int>? result);
//
//         Assert.True(success);
//         Assert.NotNull(result);
//         Assert.Equal(expected, result.Items.ToArray());
//     }
//
//     [Theory]
//     [InlineData("1, 2, 3")] // 没有包裹方括号
//     [InlineData("[1,2,3")]   // 缺失右方括号
//     [InlineData("[]]")]      // 多余括号
//     [InlineData("[{a=1}, b]")] // 非 int 类型不能正常解析
//     public void Array_TryParse_InvalidInputs(string input)
//     {
//         bool success = JarfterArray<int>.TryParse(input, CultureInfo.InvariantCulture, out var result);
//
//         Assert.False(success);
//         Assert.Null(result);
//     }
//
//     [Fact]
//     public void Array_Parse_ShouldThrow()
//     {
//         const string Input = "[1,2,invalid]";
//         Assert.Throws<InvalidCastException>(() => JarfterArray<int>.Parse(Input, CultureInfo.InvariantCulture));
//     }
//     
//     [Theory]
//     [InlineData("{ a = 1, b = 2 }", "a", 1)]
//     [InlineData("{a=42}", "a", 42)]
//     [InlineData("{ x = {1,2,3}, y = {a=1} }", "x", 3)] // x 是数组, 读取为 JarfterArray<int>, 验证元素数量
//     public void Dictionary_TryParse_ValidInputs(string input, string keyToRead, int expected)
//     {
//         bool success = JarfterDictionary.TryParse(input, CultureInfo.InvariantCulture, out var dict);
//         Assert.True(success);
//         Assert.NotNull(dict);
//
//         if (keyToRead == "x")
//         {
//             var array = dict.ReadAs<JarfterArray<int>>("x", CultureInfo.InvariantCulture);
//             Assert.NotNull(array);
//             Assert.Equal(expected, array.Items.Length);
//         }
//         else
//         {
//             var value = dict.ReadAs<int>(keyToRead, CultureInfo.InvariantCulture);
//             Assert.Equal(expected, value);
//         }
//     }
//
//     [Theory]
//     [InlineData("a = 1, b = 2")] // 缺少外层大括号
//     [InlineData("{ a = 1, b = }")] // value 缺失
//     [InlineData("{ a 1 }")] // 缺少等号
//     [InlineData("{ a = [1, 2 }, b = 3 }")] // 括号不匹配
//     [InlineData("{ a = \"unfinished }")] // 字符串未闭合
//     public void Dictionary_TryParse_InvalidInputs(string input)
//     {
//         bool success = JarfterDictionary.TryParse(input, CultureInfo.InvariantCulture, out var dict);
//         Assert.False(success);
//         Assert.Null(dict);
//     }
//
//     [Fact]
//     public void Dictionary_Parse_ShouldThrow()
//     {
//         const string Input = "{ a = 1, b = [1, invalid] }";
//         Assert.Throws<FormatException>(() => JarfterDictionary.Parse(Input, CultureInfo.InvariantCulture));
//     }
//
//     [Fact]
//     public void Dictionary_ReadAs_MissingKey_ReturnsDefault()
//     {
//         const string Input = "{ a = 1 }";
//         var dict = JarfterDictionary.Parse(Input, CultureInfo.InvariantCulture);
//         var value = dict.ReadAs<int>("missing", CultureInfo.InvariantCulture);
//         Assert.Equal(0, value); // default int
//     }
//
//     [Fact]
//     public void Dictionary_ReadAs_WithNestedDictionary()
//     {
//         const string Input = "{ config = { level = 5, enabled = true } }";
//         var dict = JarfterDictionary.Parse(Input, CultureInfo.InvariantCulture);
//
//         var nestedDictSpan = dict.Items["config"].AsSpan(Input);
//         var nestedDict = JarfterDictionary.Parse(nestedDictSpan, CultureInfo.InvariantCulture);
//
//         Assert.Equal(2, nestedDict.Items.Count);
//         Assert.Equal(5, nestedDict.ReadAs<int>("level", CultureInfo.InvariantCulture));
//         Assert.True(nestedDict.ReadAs<bool>("enabled", CultureInfo.InvariantCulture));
//     }
// }
