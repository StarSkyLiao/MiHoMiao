using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Console;

public class JarfterConsole
{
    [field: AllowNull, MaybeNull]
    private LruHashSet<string> HistoryCommand
        => field ??= new LruHashSet<string>(128);

    [field: AllowNull, MaybeNull]
    private LruIterator<string> HistoryIterator 
        => field ??= new LruIterator<string>(HistoryCommand);
    
    private readonly List<JarfterFunc> m_FuncCodes = [];
    
    [field: AllowNull, MaybeNull]
    private JarfterArray<JarfterFunc> JarfterFunc 
        => field ??= new JarfterArray<JarfterFunc>(m_FuncCodes);

    private readonly JarfterInterpreter m_Interpreter = new JarfterInterpreter();

    public JarfterConsole()
    {
        m_Interpreter.JarfterContext.CallingTree.Push(new JarfterFrame(JarfterFunc));
    }
    
    /// <summary>
    /// 执行一行语句
    /// </summary>
    public void Run(string input)
    {
#if DEBUG
        RunDebug(input);
#else 
        RunRelease(input);
#endif
    }
    
    public void RunRelease(string input)
    {
        m_FuncCodes.Add(new JarfterFunc(input));
        try
        {
            m_Interpreter.RunConsole(input);
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            m_FuncCodes.RemoveAt(m_FuncCodes.Count - 1);
        }
        finally
        {
            HistoryCommand.Add(input);
        }
    }
    
    internal void RunDebug(string input)
    {
        m_FuncCodes.Add(new JarfterFunc(input));
        m_Interpreter.RunConsole(input);
        HistoryCommand.Add(input);
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