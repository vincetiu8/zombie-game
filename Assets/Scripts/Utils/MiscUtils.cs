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
        
        /// <summary>
        /// Switch to action map, Enums should reflect actions maps that actually exists
        /// </summary>
        public enum ActionMapOptions {UI, Game, InAnimation}
        public static ActionMapOptions ToggleInput(ActionMapOptions actionMap, PlayerInput input)
        {
            input.SwitchCurrentActionMap(actionMap.ToString());
            return actionMap;
        }
    }
}