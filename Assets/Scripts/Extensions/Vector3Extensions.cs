using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{

    public static Vector3 Variation(this Vector3 v, float x, float y, float z) => Variation(v, new Vector3(x, y, z));
    public static Vector3 Variation(this Vector3 v, Vector3 variation) => new Vector3(v.x + variation.x, v.y + variation.y, v.z + variation.z);

}
