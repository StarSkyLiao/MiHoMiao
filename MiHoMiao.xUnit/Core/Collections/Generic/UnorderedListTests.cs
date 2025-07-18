using MiHoMiao.Core.Collections.Generic;

namespace MiHoMiao.xUnit.Core.Collections.Generic;

public class UnorderedListTests
{
    [Fact]
    public void Constructor_Default_CreatesEmptyList()
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        UnorderedList<int> list = [];
        Assert.Empty(list);
    }

    [Fact]
    public void Constructor_WithCapacity_SetsCapacity()
    {
        UnorderedList<int> list = new UnorderedList<int>(10);
        Assert.Empty(list);
        Assert.Equal(10, list.Capacity);
    }

    [Fact]
    public void Constructor_WithCapacity_NegativeCapacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new UnorderedList<int>(-1));
    }

    [Fact]
    public void Constructor_WithCollection_CopiesElements()
    {
        List<int> source = [1, 2, 3];
        UnorderedList<int> list = new UnorderedList<int>(source);
        Assert.Equal(3, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
        Assert.Contains(3, list);
    }

    [Fact]
    public void Constructor_WithNullCollection_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new UnorderedList<int>(null));
    }

    [Fact]
    public void Add_IncreasesCountAndAddsItem()
    {
        UnorderedList<int> list =
        [
            42
        ];
        Assert.Single(list);
        Assert.Contains(42, list);
    }

    [Fact]
    public void AddRange_AddsMultipleItems()
    {
        UnorderedList<int> list = [];
        List<int> items = [1, 2, 3];
        list.AddRange(items);
        Assert.Equal(3, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
        Assert.Contains(3, list);
    }

    [Fact]
    public void Remove_RemovesItemAndReturnsTrue()
    {
        UnorderedList<int> list = [1, 2, 3];
        bool result = list.Remove(2);
        Assert.True(result);
        Assert.Equal(2, list.Count);
        Assert.DoesNotContain(2, list);
    }

    [Fact]
    public void Remove_NonExistentItem_ReturnsFalse()
    {
        UnorderedList<int> list = [1, 2, 3];
        bool result = list.Remove(4);
        Assert.False(result);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void Clear_EmptiesList()
    {
        UnorderedList<int> list = [1, 2, 3];
        list.Clear();
        Assert.Empty(list);
    }

    [Fact]
    public void Capacity_SetValidCapacity_UpdatesCapacity()
    {
        UnorderedList<int> list = [1, 2];
        list.Capacity = 5;
        Assert.Equal(5, list.Capacity);
        Assert.Equal(2, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
    }

    [Fact]
    public void Capacity_SetLessThanCount_Throws()
    {
        UnorderedList<int> list = [1, 2, 3];
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Capacity = 2);
    }

    [Fact]
    public void Indexer_GetAndSet_ValidIndex()
    {
        UnorderedList<int> list = [1, 2, 3];
        Assert.Equal(2, list[1]);
        list[1] = 42;
        Assert.Equal(42, list[1]);
    }

    [Fact]
    public void Indexer_InvalidIndex_Throws()
    {
        UnorderedList<int> list = [1, 2, 3];
        Assert.Throws<ArgumentOutOfRangeException>(() => list[3]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[3] = 42);
    }

    [Fact]
    public void Find_ReturnsFirstMatchingItem()
    {
        UnorderedList<int> list = [1, 2, 3, 4];
        int result = list.Find(x => x > 2);
        Assert.Equal(3, result);
    }

    [Fact]
    public void FindAll_ReturnsAllMatchingItems()
    {
        UnorderedList<int> list = [1, 2, 3, 4];
        UnorderedList<int> result = list.FindAll(x => x % 2 == 0);
        Assert.Equal(2, result.Count);
        Assert.Contains(2, result);
        Assert.Contains(4, result);
    }

    [Fact]
    public void RemoveAll_RemovesMatchingItems()
    {
        UnorderedList<int> list = [1, 2, 3, 4];
        int removed = list.RemoveAll(x => x % 2 == 0);
        Assert.Equal(2, removed);
        Assert.Equal(2, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(3, list);
    }

    [Fact]
    public void ToArray_ReturnsCorrectArray()
    {
        UnorderedList<int> list = [1, 2, 3];
        int[] array = list.ToArray();
        Assert.Equal(3, array.Length);
        Assert.Contains(1, array);
        Assert.Contains(2, array);
        Assert.Contains(3, array);
    }

    [Fact]
    public void Enumerator_EnumeratesCorrectly()
    {
        UnorderedList<int> list = [1, 2, 3];
        using IEnumerator<int> enumerator = list.GetEnumerator();
        List<int> result = [];
        while (enumerator.MoveNext()) result.Add(enumerator.Current);
        Assert.Equal(3, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
    }

    [Fact]
    public void Enumerator_ModifiedDuringEnumeration_Throws()
    {
        UnorderedList<int> list = [1, 2, 3];
        using IEnumerator<int> enumerator = list.GetEnumerator();
        enumerator.MoveNext();
        list.Add(4);
        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void TrimExcess_ReducesCapacity()
    {
        UnorderedList<int> list = new UnorderedList<int>(10) { 1, 2, 3 };
        list.TrimExcess();
        Assert.Equal(3, list.Capacity);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void ConvertAll_ConvertsToNewType()
    {
        UnorderedList<int> list = [1, 2, 3];
        UnorderedList<string> converted = list.ConvertAll(x => x.ToString());
        Assert.Equal(3, converted.Count);
        Assert.Contains("1", converted);
        Assert.Contains("2", converted);
        Assert.Contains("3", converted);
    }

    [Fact]
    public void TrueForAll_ReturnsCorrectResult()
    {
        UnorderedList<int> list = [2, 4, 6];
        Assert.True(list.TrueForAll(x => x % 2 == 0));
        Assert.False(list.TrueForAll(x => x > 5));
    }
}