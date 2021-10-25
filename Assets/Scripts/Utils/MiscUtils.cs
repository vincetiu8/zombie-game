using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
	public static class MiscUtils
	{
		public delegate bool ToggleFunction(InputAction action);

		public static bool IsInLayerMask(LayerMask layerMask, int layer)
		{
			return (layerMask.value & (1 << layer)) > 0;
		}

		public static void ToggleActions(PlayerInput playerInput, string[] exceptions, bool enabled)
		{
			ToggleActions(playerInput, action =>
			                           {
				                           if (Array.Exists(exceptions, s => s == action.name)) return true;

				                           ToggleAction(action, enabled);
				                           return true;
			                           });
		}

		public static void ToggleActions(PlayerInput playerInput, ToggleFunction toggleFunction)
		{
			foreach (InputAction action in playerInput.currentActionMap.actions)
			{
				if (!toggleFunction(action)) break;
			}
		}

		public static void ToggleAction(PlayerInput playerInput, string name, bool enabled)
		{
			ToggleActions(playerInput, action =>
			                           {
				                           if (action.name != name) return true;
				                           ToggleAction(action, enabled);
				                           return false;
			                           });
		}

		private static void ToggleAction(InputAction action, bool enabled)
		{
			if (enabled)
			{
				action.Enable();
				return;
			}

			action.Disable();
		}
	}
}