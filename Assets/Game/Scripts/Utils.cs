using UnityEngine;

namespace Game.Scripts
{
    public static class Utils
    {
        public static bool IsInLayerMask(LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) > 0;
        }
    }
}