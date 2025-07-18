using MiHoMiao.Core.Collections.Generic;

namespace MiHoMiao.xUnit.Core.Collections.Generic;

public class LruHashSetTests
{
    [Fact]
    public void Add_NewItem_IncreasesCountAndContainsItem()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1 };

        Assert.Single(set);
        Assert.Contains(1, set);
    }
    
    [Fact]
    public void Add_ExistingItem_MovesToFront()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1, 2, 1 };

        int[] result = set.ToArray();
        Assert.Equal(new[] { 1, 2 }, result);
        Assert.Equal(2, set.Count);
    }
    
    [Fact]
    public void Add_OverCapacity_RemovesOldest()
    {
        LruHashSet<int> set = new LruHashSet<int>(2) { 1, 2, 3 };

        Assert.Equal(2, set.Count);
        Assert.DoesNotContain(1, set);
        Assert.Contains(2, set);
        Assert.Contains(3, set);
    }
    
    [Fact]
    public void Clear_RemovesAllItems()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1, 2, };
        set.Clear();
        
        Assert.Empty(set);
        Assert.DoesNotContain(1, set);
        Assert.DoesNotContain(2, set);
    }
    
    [Fact]
    public void Contains_NonExistingItem_ReturnsFalse()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1 };

        Assert.DoesNotContain(2, set);
    }
    
    [Fact]
    public void Remove_ExistingItem_DecreasesCount()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1 };
        bool result = set.Remove(1);
        
        Assert.True(result);
        Assert.Empty(set);
        Assert.DoesNotContain(1, set);
    }
    
    [Fact]
    public void Remove_NonExistingItem_ReturnsFalse()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1 };

        bool result = set.Remove(2);
        Assert.False(result);
        Assert.Single(set);
    }
    
    [Fact]
    public void CopyTo_CopiesItemsInOrder()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1, 2, };

        int[] array = new int[2];
        set.CopyTo(array, 0);
        
        Assert.Equal([2, 1], array);
    }
    
    [Fact]
    public void Version_IncreasesOnModification()
    {
        LruHashSet<int> set = new LruHashSet<int>(3);
        int initialVersion = set.Version;
        
        set.Add(1);
        Assert.Equal(initialVersion + 1, set.Version);
        
        set.Clear();
        Assert.Equal(initialVersion + 2, set.Version);
        
        set.Add(2);
        Assert.Equal(initialVersion + 3, set.Version);
        
        set.Remove(2);
        Assert.Equal(initialVersion + 4, set.Version);
    }
    
    [Fact]
    public void GetEnumerator_EnumeratesInInsertionOrder()
    {
        LruHashSet<int> set = new LruHashSet<int>(3) { 1, 2, 3, };

        int[] result = set.ToArray();
        Assert.Equal(new[] { 3, 2, 1 }, result);
    }
}