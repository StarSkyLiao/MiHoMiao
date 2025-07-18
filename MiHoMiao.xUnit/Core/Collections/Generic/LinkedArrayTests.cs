using MiHoMiao.Core.Collections.Generic;

namespace MiHoMiao.xUnit.Core.Collections.Generic;

public class LinkedArrayTests
{
    [Fact]
    public void Constructor_Default_CreatesEmptyList()
    {
        LinkedArray<int> list = [];
        Assert.Empty(list);
        Assert.Null(list.First);
        Assert.Null(list.Last);
    }

    [Fact]
    public void Constructor_WithCapacity_SetsCapacity()
    {
        LinkedArray<int> list = new LinkedArray<int>(10);
        Assert.Empty(list);
        Assert.Equal(10, list.Capacity);
    }

    [Fact]
    public void Constructor_WithCapacity_NegativeCapacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new LinkedArray<int>(-1));
    }

    [Fact]
    public void Constructor_WithCollection_CopiesElements()
    {
        List<int> source = [1, 2, 3];
        LinkedArray<int> list = new LinkedArray<int>(source);
        Assert.Equal(3, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
        Assert.Contains(3, list);
    }

    [Fact]
    public void Constructor_WithNullCollection_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new LinkedArray<int>(null));
    }

    [Fact]
    public void Add_IncreasesCountAndAddsItem()
    {
        LinkedArray<int> list = [42];
        Assert.Single(list);
        Assert.Equal(42, list.FirstValue);
        Assert.Equal(42, list.LastValue);
    }

    [Fact]
    public void AddRange_AddsMultipleItems()
    {
        LinkedArray<int> list = [];
        List<int> items = [1, 2, 3];
        list.AddRange(items);
        Assert.Equal(3, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
        Assert.Contains(3, list);
    }

    [Fact]
    public void AddFirst_AddsItemAtBeginning()
    {
        LinkedArray<int> list = [2, 3];
        list.AddFirst(1);
        Assert.Equal(3, list.Count);
        Assert.Equal(1, list.FirstValue);
        Assert.Equal(3, list.LastValue);
    }

    [Fact]
    public void AddAfter_AddsItemAfterNode()
    {
        LinkedArray<int> list = [1, 3];
        LinkedArrayNode<int>? node = list.Find(1);
        list.AddAfter(node!.Value, 2);
        Assert.Equal(3, list.Count);
        using IEnumerator<int> enumerator = list.GetEnumerator();
        enumerator.MoveNext();
        Assert.Equal(1, enumerator.Current);
        enumerator.MoveNext();
        Assert.Equal(2, enumerator.Current);
        enumerator.MoveNext();
        Assert.Equal(3, enumerator.Current);
    }

    [Fact]
    public void AddAfter_InvalidNode_Throws()
    {
        LinkedArray<int> list = [1];
        LinkedArray<int> otherList = [2];
        LinkedArrayNode<int>? node = otherList.First;
        Assert.Throws<InvalidOperationException>(() => list.AddAfter(node!.Value, 3));
    }

    [Fact]
    public void AddBefore_AddsItemBeforeNode()
    {
        LinkedArray<int> list = [1, 3];
        LinkedArrayNode<int>? node = list.Find(3);
        list.AddBefore(node!.Value, 2);
        Assert.Equal(3, list.Count);
        using IEnumerator<int> enumerator = list.GetEnumerator();
        enumerator.MoveNext();
        Assert.Equal(1, enumerator.Current);
        enumerator.MoveNext();
        Assert.Equal(2, enumerator.Current);
        enumerator.MoveNext();
        Assert.Equal(3, enumerator.Current);
    }

    [Fact]
    public void Remove_RemovesItemAndReturnsTrue()
    {
        LinkedArray<int> list = [1, 2, 3];
        bool result = list.Remove(2);
        Assert.True(result);
        Assert.Equal(2, list.Count);
        Assert.DoesNotContain(2, list);
    }

    [Fact]
    public void Remove_NonExistentItem_ReturnsFalse()
    {
        LinkedArray<int> list = [1, 2, 3];
        bool result = list.Remove(4);
        Assert.False(result);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void RemoveFirst_RemovesFirstItem()
    {
        LinkedArray<int> list = [1, 2, 3];
        bool result = list.RemoveFirst();
        Assert.True(result);
        Assert.Equal(2, list.Count);
        Assert.Equal(2, list.FirstValue);
    }

    [Fact]
    public void RemoveLast_RemovesLastItem()
    {
        LinkedArray<int> list = [1, 2, 3];
        bool result = list.RemoveLast();
        Assert.True(result);
        Assert.Equal(2, list.Count);
        Assert.Equal(2, list.LastValue);
    }

    [Fact]
    public void TryDequeue_RemovesAndReturnsFirstItem()
    {
        LinkedArray<int> list = [1, 2, 3];
        bool result = list.TryDequeue(out int item);
        Assert.True(result);
        Assert.Equal(1, item);
        Assert.Equal(2, list.Count);
        Assert.Equal(2, list.FirstValue);
    }

    [Fact]
    public void Clear_EmptiesList()
    {
        LinkedArray<int> list = [1, 2, 3];
        list.Clear();
        Assert.Empty(list);
        Assert.Null(list.First);
        Assert.Null(list.Last);
    }

    [Fact]
    public void Capacity_SetValidCapacity_UpdatesCapacity()
    {
        LinkedArray<int> list = [1, 2];
        list.Capacity = 5;
        Assert.Equal(5, list.Capacity);
        Assert.Equal(2, list.Count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
    }

    [Fact]
    public void Capacity_SetLessThanCount_Throws()
    {
        LinkedArray<int> list = [1, 2, 3];
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Capacity = 2);
    }

    [Fact]
    [Obsolete("Obsolete")]
    public void Indexer_GetAndSet_ValidIndex()
    {
        LinkedArray<int> list = [1, 2, 3];
        Assert.Equal(1, list[0]);
        list[0] = 42;
        Assert.Equal(42, list[0]);
    }

    [Fact]
    [Obsolete("Obsolete")]
    public void Indexer_InvalidIndex_Throws()
    {
        LinkedArray<int> list = [1, 2, 3];
        Assert.Throws<ArgumentOutOfRangeException>(() => list[3]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[3] = 42);
    }

    [Fact]
    public void Find_ReturnsFirstMatchingNode()
    {
        LinkedArray<int> list = [1, 2, 3, 2];
        LinkedArrayNode<int>? node = list.Find(2);
        Assert.NotNull(node);
        Assert.Equal(2, node.Value.Value);
    }

    [Fact]
    public void FindLast_ReturnsLastMatchingNode()
    {
        LinkedArray<int> list = [1, 2, 3, 2];
        LinkedArrayNode<int>? node = list.FindLast(2);
        Assert.NotNull(node);
        Assert.Equal(2, node.Value.Value);
    }

    [Fact]
    public void Enumerator_EnumeratesCorrectly()
    {
        LinkedArray<int> list = [1, 2, 3];
        using IEnumerator<int> enumerator = list.GetEnumerator();
        List<int> result = [];
        while (enumerator.MoveNext())
        {
            result.Add(enumerator.Current);
        }
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(3, result[2]);
    }

    [Fact]
    public void Enumerator_ModifiedDuringEnumeration_Throws()
    {
        LinkedArray<int> list = [1, 2, 3];
        using IEnumerator<int> enumerator = list.GetEnumerator();
        enumerator.MoveNext();
        list.Add(4);
        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void TrimExcess_ReducesCapacity()
    {
        LinkedArray<int> list = new LinkedArray<int>(10) { 1, 2, 3 };
        list.TrimExcess();
        Assert.Equal(4, list.Capacity);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void ConvertAll_ConvertsToNewType()
    {
        LinkedArray<int> list = [1, 2, 3];
        List<string> converted = list.ConvertAll(x => x.ToString());
        Assert.Equal(3, converted.Count);
        Assert.Contains("1", converted);
        Assert.Contains("2", converted);
        Assert.Contains("3", converted);
    }

    [Fact]
    public void TrueForAll_ReturnsCorrectResult()
    {
        LinkedArray<int> list = [2, 4, 6];
        Assert.True(list.TrueForAll(x => x % 2 == 0));
        Assert.False(list.TrueForAll(x => x > 5));
    }
}