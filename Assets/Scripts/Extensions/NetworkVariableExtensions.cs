using System;
using System.Collections.Generic;
using Unity.Netcode;

internal static class NetworkVariableExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(this NetworkList<T> list)
        where T : unmanaged, IEquatable<T>
    {
        foreach (var item in list)
        {
            yield return item;
        }
    }

    public static bool IsAddOrInsert<T>(this NetworkListEvent<T>.EventType type)
    {
        return type == NetworkListEvent<T>.EventType.Add || type == NetworkListEvent<T>.EventType.Insert;
    }

    public static bool IsRemoveOrRemoveAt<T>(this NetworkListEvent<T>.EventType type)
    {
        return type == NetworkListEvent<T>.EventType.Remove || type == NetworkListEvent<T>.EventType.RemoveAt;
    }
}
