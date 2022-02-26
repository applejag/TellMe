using System;
using System.Collections.Generic;

internal static class LinqExtensions
{
    public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (predicate(item))
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}
