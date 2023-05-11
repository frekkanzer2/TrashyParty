using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{

    public static Vector3 Variation(this Vector3 v, float x, float y, float z) => Variation(v, new Vector3(x, y, z));
    public static Vector3 Variation(this Vector3 v, Vector3 variation) => new Vector3(v.x + variation.x, v.y + variation.y, v.z + variation.z);
}

namespace Extensions
{
    public static class Vector3
    {
        public static UnityEngine.Vector3 GenerateWithRandomValues(Vector2 xRange, Vector2 yRange, Vector2 zRange)
        => new(Random.Range(xRange.x, xRange.y), Random.Range(yRange.x, yRange.y), Random.Range(zRange.x, zRange.y));
    }
}
