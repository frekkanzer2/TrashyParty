using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{

    public static Vector3 Variation(this Vector3 v, float x, float y, float z) => Variation(v, new Vector3(x, y, z));
    public static Vector3 Variation(this Vector3 v, Vector3 variation) => new Vector3(v.x + variation.x, v.y + variation.y, v.z + variation.z);
    public static Vector3 Opposite(this Vector3 v, bool invertX, bool invertY, bool invertZ) => new Vector3((invertX) ? v.x * -1 : v.x, (invertY) ? v.y * -1 : v.y, (invertZ) ? v.z * -1 : v.z);
    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);
}

namespace Extensions
{
    public static class Vector3
    {
        public static UnityEngine.Vector3 GenerateWithRandomValues(Vector2 xRange, Vector2 yRange, Vector2 zRange)
        => new(Random.Range(xRange.x, xRange.y), Random.Range(yRange.x, yRange.y), Random.Range(zRange.x, zRange.y));
        public static UnityEngine.Vector3 FromVector2(Vector2 v)
        => new(v.x, v.y, 0);
        public static UnityEngine.Vector3 FromVector2(Vector2 v, float z)
        => new(v.x, v.y, z);
    }
}
