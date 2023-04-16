using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppSettings
{
    private static Dictionary<string, object> values = new Dictionary<string, object>();
    public static void Save(string key, object value) => values.Add(key, value);
    public static object? Get(string key) => values[key];
}
