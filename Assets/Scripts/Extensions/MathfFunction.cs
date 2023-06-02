using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfFunction
{
    public static float StraightLine(float a, float b, float x) => a * x + b;
    public static float Quadratic(float x) => Mathf.Pow(x, 2); // x^2
    public static float Cubic(float x) => Mathf.Pow(x, 3); // x^3
    public static float SquareRoot(float x)
    {
        if (x <= 0) throw new ArgumentException("x cannot be negative");
        return Mathf.Sqrt(x);
    }
    public static float CubeRoot(float x) => (float)System.Math.Cbrt(x);
    public static float Exponential(float a, float x)
    {
        if (a <= 0) throw new ArgumentException("a cannot be negative");
        return (float)Math.Pow(Convert.ToDouble(a), Convert.ToDouble(x)); // a^x
    }
    public static float Exponential(float x) => Exponential((float)System.Math.E, x); // e^x
    public static float Power(float a, float x) => Mathf.Pow(x, a); // x^a
    public static float Logarithmic(float x) => Mathf.Log(x, (float)System.Math.E); // log of x with base E
}
