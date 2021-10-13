using UnityEngine;

namespace Utils
{
	public static class MiscUtils
	{
		public static bool IsInLayerMask(LayerMask layerMask, int layer)
		{
			return (layerMask.value & (1 << layer)) > 0;
		}
	}
}