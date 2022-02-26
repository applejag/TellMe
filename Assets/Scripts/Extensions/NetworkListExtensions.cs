using System;
using System.Collections.Generic;
using Unity.Netcode;

internal static class NetworkListExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(this NetworkList<T> list)
        where T : unmanaged, IEquatable<T>
    {
        foreach (var item in list)
        {
            yield return item;
        }
    }
}
