using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private static System.Random rng = new System.Random();

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

    public static void DestroyChildren(this Transform toDestroy)
    {
        while (toDestroy.childCount > 0)
        {
            Transform child = toDestroy.GetChild(0);
            if (Application.isEditor)
            {
                Debug.Log(child.name);
                GameObject.DestroyImmediate(child.gameObject);
            }
            else
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}