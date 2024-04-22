using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetExtensionMethodsSeed(int seed)
    {
        Random.InitState(seed);
        //rng = new System.Random(seed);
    }

    //Shuffle using the System.Random fundtions instead of using Unitys counterpart
    //private static System.Random rng = new System.Random();
    //public static void Shuffle<T>(this IList<T> list)
    //{
    //    int n = list.Count;
    //    while (n > 1)
    //    {
    //        n--;
    //        int k = rng.Next(n + 1);
    //        T value = list[k];
    //        list[k] = list[n];
    //        list[n] = value;
    //    }
    //}

    /// <summary>
    /// Shuffles the List randomly
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Splits the IList into sections of a fixed size. Example: Your List has 12 entries and should be split into Sections with a size of 5 -> returns an array with three lists which contain 5, 5 and lastly 2 entries!
    /// </summary>
    public static List<T>[] SplitIntoFixedSections<T>(this IList<T> source, int itemsPerSection)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / itemsPerSection)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToArray();
    }

    /// <summary>
    /// Splits the IList into sections of an equal size. Example: Your List has 12 entries and should be split into 5 Sections-> returns an array with five lists which contain 3, 3, 2, 2 and lastly 2 entries!
    /// </summary>
    public static List<T>[] SplitIntoEvenSections<T>(this IList<T> originalList, int sections)
    {
        if(sections <= 1) { Debug.LogError("Sections needs to be larger than 1!"); return null; }

        List<T>[] ts = new List<T>[sections];
        for (int i = 0; i < ts.Length; i++)
        {
            ts[i] = new List<T>();
        }

        int y = 0;

        int rest = originalList.Count % sections;
        for (int i = 0; i < sections; i++)
        {
            int amount = Mathf.FloorToInt(originalList.Count / sections);
            if (rest > 0)
            {
                amount++;
                rest--;
            }

            for (int x = 0; x < amount; x++)
            {
                ts[i].Add(originalList[y]);
                y++;
            }
        }
        return ts;
    }
}

