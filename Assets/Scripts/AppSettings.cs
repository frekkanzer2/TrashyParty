using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppSettings
{
    private static readonly Dictionary<string, object> values = new();
    public static void Save(string key, object value) => values.Add(key, value);
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
    public static object? Get(string key) => values[key];
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
}
