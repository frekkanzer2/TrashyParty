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

}
