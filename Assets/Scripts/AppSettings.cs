using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppSettings
{
    private static Dictionary<string, object> values = new Dictionary<string, object>();
    public static void Save(string key, object value)
    {
        try
        {
            values.Add(key, value);
        }
        catch (System.ArgumentException)
        {
            values[key] = value;
        }
    }
    public static object? Get(string key) { 
        try
        {
            return values[key];
        } catch (KeyNotFoundException)
        {
            return null;
        }
    }
}
