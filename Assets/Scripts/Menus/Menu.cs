using UnityEngine;

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

		public void Toggle(bool active)
		{
			gameObject.SetActive(active);
			inputToggle = !inputToggle;
		}
	}
}