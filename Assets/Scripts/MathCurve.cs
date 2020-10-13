using System;
using UnityEngine;

public class MathCurve
{
    public static Vector3 Curve(Vector3 aStart, Vector3 aEnd, float aHeight, float aTime)
    {
        Func<float, float> f = x => -4 * aHeight * x * x + 4 * aHeight * x;

        var mid = Vector3.Lerp(aStart, aEnd, aTime);

        return new Vector3(mid.x, f(aTime) + Mathf.Lerp(aStart.y, aEnd.y, aTime), mid.z);
    }

    public static Vector2 Curve(Vector2 aStart, Vector2 aEnd, float aHeight, float aTime)
    {
        Func<float, float> f = x => -4 * aHeight * x * x + 4 * aHeight * x;

        var mid = Vector2.Lerp(aStart, aEnd, aTime);

        return new Vector2(mid.x, f(aTime) + Mathf.Lerp(aStart.y, aEnd.y, aTime));
    }
}
