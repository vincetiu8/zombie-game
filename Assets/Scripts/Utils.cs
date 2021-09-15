using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsInLayerMask(LayerMask layerMask, int layer)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }

    public static float Vector2ToDeg(Vector2 vector2)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vector2.y, vector2.x);
    }
}