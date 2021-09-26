using UnityEngine;

namespace Lobby
{
	/// <summary>
	///     Handles each menu.
	///     Provides functionality to toggle them on or off.
	/// </summary>
	public class Menu : MonoBehaviour
	{
		public string menuName;

		public void Toggle(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}