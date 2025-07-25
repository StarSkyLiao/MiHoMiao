using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Core.Collections.Generic;

public static class Singleton<T> where T : new()
{
    [field: AllowNull, MaybeNull]
    public static T Instance => field ??= new T();
    
}