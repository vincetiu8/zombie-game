using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
	public static class MiscUtils
	{
		public static bool IsInLayerMask(LayerMask layerMask, int layer)
		{
			return (layerMask.value & (1 << layer)) > 0;
		}

		public static bool ToggleInput(bool isPaused, PlayerInput input)
		{
			string actionMap = isPaused ? "UI" : "Game";
			input.SwitchCurrentActionMap(actionMap);
			return isPaused;
		}
	}
}