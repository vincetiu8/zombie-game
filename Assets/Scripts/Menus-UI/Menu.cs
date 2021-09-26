using Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menus_UI
{
	/// <summary>
	///     Handles each menu.
	///     Provides functionality to toggle them on or off.
	/// </summary>
	public class Menu : MonoBehaviour
	{
		public string menuName;
		
		//whether menu can disable player input 
		//upon activation
		public bool freezeOnToggle;
		private PlayerInput _playerInput;

		public void Toggle(bool active)
		{
			gameObject.SetActive(active);
			if (freezeOnToggle)
			{
				_playerInput = GameManager.instance.localPlayer.GetComponent<PlayerInput>();
				Utils.DisableInput(active, _playerInput);
			}
		}
	}
}