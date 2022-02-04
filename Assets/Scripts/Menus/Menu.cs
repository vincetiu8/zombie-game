using UnityEngine;

namespace Menus
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
		[HideInInspector] public bool inputToggle;

		public virtual void Toggle(bool active)
		{
			gameObject.SetActive(active);
			inputToggle = !inputToggle;
		}
	}
}