﻿using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Operator;

[UsedImplicitly]
public class DecFunc : IJarfterFunc<DecFunc, string>
{
    public string[] JarfterFuncName { get; } = ["dec", "math"];
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        JarfterObject value = jarfterContext.JarfterSymbolTable.LoadVariable(arg0);
        decimal number = JarfterObject.Convert<decimal>(value, arg0, jarfterContext);
        jarfterContext.JarfterSymbolTable.StoreVariable(arg0, new JarfterUnit(--number));
    }
    
}
