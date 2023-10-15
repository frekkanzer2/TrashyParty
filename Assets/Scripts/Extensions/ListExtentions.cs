using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtentions
{
    private static System.Random rng = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        System.Random rnd = new();
        return list[rnd.Next(list.Count)];
    }

    public static T GetAndRemove<T>(this IList<T> list, int index)
    {
        lock (list)
        {
            T value = list[index];
            list.RemoveAt(index);
            return value;
        }
    }

    public static T GetRandomAndRemove<T>(this IList<T> list)
    {
        lock (list)
        {
            System.Random rnd = new();
            int index = rnd.Next(list.Count);
            return GetAndRemove(list, index);
        }
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list)
        => list == null || list.Count == 0;

}
