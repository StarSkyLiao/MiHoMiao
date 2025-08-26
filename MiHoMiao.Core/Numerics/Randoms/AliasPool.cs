//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using MiHoMiao.Core.Numerics.Hash;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.Core.Numerics.Randoms;

/// <summary>
/// 基于 AliasMethod 实现的加权随机选取池.
/// 更新操作的时间复杂度为 O(n), 选取操作的时间复杂度为 O(1).
/// 优先选择封装好的<see cref="RandomPool{T}"/>>类
/// </summary> 
public class AliasPool<T>
{
    private (double Prob, int Index)[] m_Alias = null!;
    private (T item, double weight)[] m_ChanceTable = null!;
    public int Count;
    public AliasPool(ICollection<(T item, double weight)> chanceTable) => Update(chanceTable);

    /// <summary>
    ///     更新到 newChanceTable 对应的权值表
    /// </summary>
    public void Update(ICollection<(T item, double weight)> newChanceTable)
    {
        Count = 0;

        if (m_ChanceTable == null! || m_ChanceTable.Length < newChanceTable.Count)
            m_ChanceTable = new (T item, double weight)[newChanceTable.Count];
        foreach ((T item, double weight) item in newChanceTable) m_ChanceTable[Count++] = item;
        
        double sum = 0;
        for (var i = 0; i < Count; i++) sum += m_ChanceTable[i].weight;
        for (var i = 0; i < Count; i++) m_ChanceTable[i].weight /= sum;

        Queue<int> smaller = new Queue<int>(Count);
        Queue<int> larger = new Queue<int>(Count);

        for (var i = 0; i < Count; i++)
        {
            m_ChanceTable[i].weight *= Count;
            if (m_ChanceTable[i].weight < 1) smaller.Enqueue(i);
            else larger.Enqueue(i);
        }

        m_Alias = new (double, int)[Count];

        while (smaller.Count > 0 && larger.Count > 0)
        {
            int smallIndex = smaller.Dequeue();
            int largeIndex = larger.Dequeue();
            m_Alias[smallIndex] = (m_ChanceTable[smallIndex].weight, largeIndex);
            m_ChanceTable[largeIndex].weight = m_ChanceTable[smallIndex].weight + m_ChanceTable[largeIndex].weight - 1;
            if (m_ChanceTable[largeIndex].weight < 1) smaller.Enqueue(largeIndex);
            else larger.Enqueue(largeIndex);
        }

        while (larger.Count > 0)
        {
            int largeIndex = larger.Dequeue();
            m_Alias[largeIndex].Prob = 1;
        }

        while (smaller.Count > 0)
        {
            int smallIndex = smaller.Dequeue();
            m_Alias[smallIndex].Prob = 1;
        }
    }

    /// <summary>
    /// 按照权重, 随机返回一个元素
    /// </summary>
    public T GetRandomly()
    {
        int n = Randomization.Range(0, Count);
        return Randomization.Try(m_Alias[n].Prob) ? m_ChanceTable[n].item : m_ChanceTable[m_Alias[n].Index].item;
    }
    
    /// <summary>
    /// 按照权重, 随机返回一个元素
    /// </summary>
    public T GetRandomly(int seed)
    {
        int n = HashRandomUtil.Range(seed, 0, Count);
        return Randomization.Try(m_Alias[n].Prob) ? m_ChanceTable[n].item : m_ChanceTable[m_Alias[n].Index].item;
    }
}