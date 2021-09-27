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
		// This variable is solely for toggling input
		[HideInInspector] public bool inputToggle = false;
		private PlayerInput _playerInput;

		public void Toggle(bool active)
		{
			gameObject.SetActive(active); 
			inputToggle = !inputToggle;
		}
	}
}