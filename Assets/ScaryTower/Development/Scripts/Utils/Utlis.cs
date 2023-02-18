using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatUtils
{
    public static float Remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
}

public static class Vector2Utils
{
    public static Vector3 ScreenToWorld(Camera cam, Vector3 pos)
    {
        pos.z = cam.nearClipPlane;
        return cam.ScreenToWorldPoint(pos);
    }
}