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

		/*public static bool ToggleInput(bool isPaused, PlayerInput input)
		{
			string actionMap = isPaused ? "UI" : "Game";
			input.SwitchCurrentActionMap(actionMap);
			return isPaused;
		}*/
        
        public enum ActionMapOptions {UI, Game, InAnimation}
        public static ActionMapOptions ToggleInput(ActionMapOptions actionMap, PlayerInput input)
        {
            input.SwitchCurrentActionMap(actionMap.ToString());
                Debug.Log(actionMap);
            return actionMap;
        }
    }
}