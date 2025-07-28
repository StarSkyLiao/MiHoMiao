using MiHoMiao.Core.Collections.Generic;

namespace MiHoMiao.xUnit.Core.Collections.Generic;

public class LruHashSetIteratorTests
{
    private static LruHashSet<string> CreateTestSet()
    {
        LruHashSet<string> set = new LruHashSet<string>(3)
        {
            "A",
            "B",
            "C"
        };
        return set;
    }

    [Fact]
    public void Constructor_InitializesAtNewestElement()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        Assert.Equal("C", iterator.Current);
        Assert.True(iterator.IsAtFirst);
        Assert.False(iterator.IsAtLast);
    }

    [Fact]
    public void MoveNext_MovesToOlderElements()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        Assert.True(iterator.MoveNext());
        Assert.Equal("B", iterator.Current);
        Assert.False(iterator.IsAtFirst);
        Assert.False(iterator.IsAtLast);

        Assert.True(iterator.MoveNext());
        Assert.Equal("A", iterator.Current);
        Assert.False(iterator.IsAtFirst);
        Assert.True(iterator.IsAtLast);
    }

    [Fact]
    public void MoveFront_MovesToNewerElements()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        Assert.True(iterator.MoveNext());
        Assert.True(iterator.MoveNext()); // At "A"
        Assert.True(iterator.MoveFront());
        Assert.Equal("B", iterator.Current);
        Assert.True(iterator.MoveFront());
        Assert.Equal("C", iterator.Current);
        Assert.True(iterator.IsAtFirst);
    }

    [Fact]
    public void MoveNext_AtLast_ReturnsFalse()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        iterator.MoveNext();
        iterator.MoveNext(); // At last element
        Assert.False(iterator.MoveNext());
        Assert.Equal("A", iterator.Current);
        Assert.True(iterator.IsAtLast);
    }

    [Fact]
    public void MoveFront_AtFirst_ReturnsFalse()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        Assert.False(iterator.MoveFront());
        Assert.Equal("C", iterator.Current);
        Assert.True(iterator.IsAtFirst);
    }

    [Fact]
    public void VersionChange_ResetsToNewest()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        iterator.MoveNext(); // At "B"
        set.Add("D"); // Changes version
        Assert.Equal("D", iterator.Current); // Should reset to newest
        Assert.True(iterator.IsAtFirst);
    }

    [Fact]
    public void Reset_SetsToNewestElement()
    {
        LruHashSet<string> set = CreateTestSet();
        LruIterator<string> iterator = new LruIterator<string>(set);

        iterator.MoveNext();
        iterator.MoveNext(); // At "A"
        iterator.Reset();
        Assert.Equal("C", iterator.Current);
        Assert.True(iterator.IsAtFirst);
    }

    [Fact]
    public void Current_EmptySet_ThrowsInvalidOperationException()
    {
        LruHashSet<string> set = new LruHashSet<string>(3);
        LruIterator<string> iterator = new LruIterator<string>(set);

        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
    }

    [Fact]
    public void EmptySet_IteratorProperties()
    {
        LruHashSet<string> set = new LruHashSet<string>(3);
        LruIterator<string> iterator = new LruIterator<string>(set);

        Assert.False(iterator.MoveNext());
        Assert.False(iterator.MoveFront());
        Assert.False(iterator.IsAtFirst);
        Assert.False(iterator.IsAtLast);
    }

    [Fact]
    public void Constructor_NullCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LruIterator<string>(null!));
    }
}