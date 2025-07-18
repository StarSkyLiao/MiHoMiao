using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;

namespace MiHoMiao.xUnit.Jarfter.Runtime.Collection;

public class JarfterArrayTests
{

    [Fact]
    public void Test1_BasicArray_Integers() => Assert.Equal([1, 2, 3], JarfterArray<int>.ParseInternal("[1, 2, 3]").Content.ToArray());

    [Fact]
    public void Test2_EmptyArray() => Assert.Empty(JarfterArray<int>.ParseInternal("[]").Content.ToArray());

    [Fact]
    public void Test3_TrailingComma() => Assert.Equal([1, 2,], JarfterArray<int>.ParseInternal("[1, 2,]").Content.ToArray());

    [Fact]
    public void Test4_SingleElement() => Assert.Equal([42], JarfterArray<int>.ParseInternal("[42]").Content.ToArray());
    
    [Fact]
    public void Test5_NestedArray() =>
        Assert.Equal(
            [
                new JarfterArray<int>([1, 2]),
                new JarfterArray<int>([2, 3]),
                new JarfterArray<int>([1, 2])
            ],
            JarfterArray<JarfterArray<int>>.ParseInternal("[[1, 2], [2, 3], [1, 2]]").Content.ToArray()
        );
    
    [Fact]
    public void Test8_InvalidInput_MissingClosingBracket_ThrowsParseException() => 
        Assert.Throws<UnBalancedArrayException>(() => JarfterArray<int>.ParseInternal("[1, 2"));
    
    [Fact]
    public void Test9_InvalidInput_NonParsableElement_ThrowsParseException() => 
        Assert.Throws<FormatException>(() => JarfterArray<int>.ParseInternal("[1, abc, 3]"));
    
    [Fact]
    public void Test10_InvalidInput_UnmatchedBracket_ThrowsParseException() => 
        Assert.Throws<UnBalancedArrayException>(() => JarfterArray<int>.ParseInternal("][1, 2]"));
}