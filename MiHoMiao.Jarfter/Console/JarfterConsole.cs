using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Console;

public class JarfterConsole
{
    /// <summary>
    /// 历史记录的指令, 用于上下切换
    /// </summary>
    [field: AllowNull, MaybeNull]
    private LruHashSet<string> HistoryCommand => field ??= new LruHashSet<string>(128);

    /// <summary>
    /// 用于迭代历史记录的迭代器
    /// </summary>
    [field: AllowNull, MaybeNull]
    private LruIterator<string> HistoryIterator => field ??= new LruIterator<string>(HistoryCommand);
    
    /// <summary>
    /// Jarfter 的解释器
    /// </summary>
    public readonly JarfterInterpreter Interpreter = new JarfterInterpreter();
    
    /// <summary>
    /// 执行环境的上下文
    /// </summary>
    private readonly List<JarfterFunc> m_FuncCodes = [];

    public JarfterConsole() => Interpreter.JarfterContext.CallingTree.Push(
        new JarfterFrame(new JarfterArray<JarfterFunc>(m_FuncCodes))
    );

    /// <summary>
    /// 执行一行语句
    /// </summary>
    public void Run(string input)
    {
        m_FuncCodes.Add(new JarfterFunc(input));
        try
        {
            Interpreter.RunConsole(input);
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            m_FuncCodes.RemoveAt(m_FuncCodes.Count - 1);
            throw;
        }
        finally
        {
            HistoryCommand.Add(input);
        }
    }
    
    /// <summary>
    /// 获取更早的输入记录
    /// </summary>
    public string LastInput()
    {
        string item = HistoryIterator.Current;
        HistoryIterator.MoveNext();
        return item;
    }
    
    /// <summary>
    /// 获取更晚的输入记录
    /// </summary>
    public string NextInput()
    {
        string item = HistoryIterator.Current;
        HistoryIterator.MoveFront();
        return item;
    }
    
}