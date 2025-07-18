using MiHoMiao.Core.Collections.Generic;

namespace MiHoMiao.xUnit.Core.Collections.Generic;

public class ArrayDequeTests
{
    [Fact]
    public void Constructor_Default_CreatesEmptyDeque()
    {
        ArrayDeque<int> deque = [];
        Assert.Empty(deque);
        Assert.Empty(deque.ToArray());
    }

    [Fact]
    public void Constructor_WithCapacity_SetsCapacity()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(5);
        Assert.Empty(deque);
        Assert.Equal(5, deque.Capacity);
    }

    [Fact]
    public void Constructor_WithCollection_InitializesCorrectly()
    {
        int[] items = [1, 2, 3];
        ArrayDeque<int> deque = new ArrayDeque<int>(items);
        Assert.Equal(3, deque.Count);
        Assert.Equal(items, deque.ToArray());
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ArrayDeque<int>(-1));
    }

    [Fact]
    public void Constructor_WithNullCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ArrayDeque<int>(null!));
    }

    [Fact]
    public void AddLast_AddsItemToEnd()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(3);
        deque.AddLast(1);
        deque.AddLast(2);
        Assert.Equal(2, deque.Count);
        Assert.Equal(new[] { 1, 2 }, deque.ToArray());
    }

    [Fact]
    public void AddFirst_AddsItemToBeginning()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(3);
        deque.AddFirst(1);
        deque.AddFirst(2);
        Assert.Equal(2, deque.Count);
        Assert.Equal(new[] { 2, 1 }, deque.ToArray());
    }

    [Fact]
    public void Enqueue_AddsItemToEnd()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(3);
        deque.Enqueue(1);
        deque.Enqueue(2);
        Assert.Equal(2, deque.Count);
        Assert.Equal(new[] { 1, 2 }, deque.ToArray());
    }

    [Fact]
    public void OfferFirst_AddsItemToBeginning()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(3);
        deque.OfferFirst(1);
        deque.OfferFirst(2);
        Assert.Equal(2, deque.Count);
        Assert.Equal(new[] { 2, 1 }, deque.ToArray());
    }

    [Fact]
    public void Dequeue_RemovesAndReturnsFirstItem()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int result = deque.Dequeue();
        Assert.Equal(1, result);
        Assert.Equal(2, deque.Count);
        Assert.Equal(new[] { 2, 3 }, deque.ToArray());
    }

    [Fact]
    public void Dequeue_EmptyDeque_ThrowsInvalidOperationException()
    {
        ArrayDeque<int> deque = [];
        Assert.Throws<InvalidOperationException>(() => deque.Dequeue());
    }

    [Fact]
    public void PollLast_RemovesAndReturnsLastItem()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int result = deque.PollLast();
        Assert.Equal(3, result);
        Assert.Equal(2, deque.Count);
        Assert.Equal(new[] { 1, 2 }, deque.ToArray());
    }

    [Fact]
    public void TryDequeue_EmptyDeque_ReturnsFalse()
    {
        ArrayDeque<int> deque = [];
        bool result = deque.TryDequeue(out int item);
        Assert.False(result);
        Assert.Equal(0, item);
    }

    [Fact]
    public void TryPollLast_NonEmptyDeque_ReturnsTrueAndLastItem()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        bool result = deque.TryPollLast(out int item);
        Assert.True(result);
        Assert.Equal(3, item);
        Assert.Equal(2, deque.Count);
    }

    [Fact]
    public void PeekFirst_ReturnsFirstItemWithoutRemoving()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int result = deque.PeekFirst();
        Assert.Equal(1, result);
        Assert.Equal(3, deque.Count);
    }

    [Fact]
    public void PeekLast_ReturnsLastItemWithoutRemoving()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int result = deque.PeekLast();
        Assert.Equal(3, result);
        Assert.Equal(3, deque.Count);
    }

    [Fact]
    public void TryPeek_EmptyDeque_ReturnsFalse()
    {
        ArrayDeque<int> deque = [];
        bool result = deque.TryPeek(out int item);
        Assert.False(result);
        Assert.Equal(0, item);
    }

    [Fact]
    public void TryPeekLast_NonEmptyDeque_ReturnsTrueAndLastItem()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        bool result = deque.TryPeekLast(out int item);
        Assert.True(result);
        Assert.Equal(3, item);
        Assert.Equal(3, deque.Count);
    }

    [Fact]
    public void Contains_ExistingItem_ReturnsTrue()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        Assert.Contains(2, deque);
    }

    [Fact]
    public void Contains_NonExistingItem_ReturnsFalse()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        Assert.DoesNotContain(4, deque);
    }

    [Fact]
    public void Clear_ResetsDeque()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        deque.Clear();
        Assert.Empty(deque);
        Assert.Empty(deque.ToArray());
    }

    [Fact]
    public void CopyTo_CopiesItemsInCorrectOrder()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int[] array = new int[3];
        deque.CopyTo(array, 0);
        Assert.Equal(new[] { 1, 2, 3 }, array);
    }

    [Fact]
    public void CopyTo_InvalidArrayIndex_ThrowsArgumentOutOfRangeException()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int[] array = new int[3];
        Assert.Throws<ArgumentOutOfRangeException>(() => deque.CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_InsufficientArraySize_ThrowsArgumentOutOfRangeException()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        int[] array = new int[2];
        Assert.Throws<ArgumentOutOfRangeException>(() => deque.CopyTo(array, 0));
    }

    [Fact]
    public void Remove_ThrowsInvalidOperationException()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        Assert.Throws<InvalidOperationException>(() => deque.Remove(1));
    }

    [Fact]
    public void TrimExcess_ReducesCapacityToCount()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(10);
        deque.AddLast(1);
        deque.AddLast(2);
        deque.TrimExcess();
        Assert.Equal(2, deque.Capacity);
        Assert.Equal(new[] { 1, 2 }, deque.ToArray());
    }

    [Fact]
    public void TrimExcess_WithCapacity_SetsNewCapacity()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(10);
        deque.AddLast(1);
        deque.AddLast(2);
        deque.TrimExcess(5);
        Assert.Equal(5, deque.Capacity);
        Assert.Equal(new[] { 1, 2 }, deque.ToArray());
    }

    [Fact]
    public void EnsureCapacity_IncreasesCapacityWhenNeeded()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(2);
        int newCapacity = deque.EnsureCapacity(5);
        Assert.True(newCapacity >= 5);
        Assert.Empty(deque);
    }

    [Fact]
    public void GetEnumerator_EnumeratesInCorrectOrder()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        List<int> result = [];
        foreach (int item in deque)
        {
            result.Add(item);
        }
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void GetEnumerator_CollectionModified_ThrowsInvalidOperationException()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>([1, 2, 3]);
        using ArrayDeque<int>.Enumerator enumerator = deque.GetEnumerator();
        enumerator.MoveNext();
        deque.AddLast(4);
        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void WrapAround_AddAndRemove_HandlesCorrectly()
    {
        ArrayDeque<int> deque = new ArrayDeque<int>(3);
        deque.AddLast(1);
        deque.AddLast(2);
        deque.Dequeue(); // Remove 1
        deque.AddLast(3);
        deque.AddLast(4);
        Assert.Equal(new[] { 2, 3, 4 }, deque.ToArray());
    }
}