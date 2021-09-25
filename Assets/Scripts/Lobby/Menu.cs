using UnityEngine;

namespace Lobby
{
	// Menu handles each menu
	// Provides functionality to turn them on or off
	public class Menu : MonoBehaviour
	{
		#region Variables

		public string menuName;

		#endregion

		#region Methods

		public void Toggle(bool active)
		{
			gameObject.SetActive(active);
		}

		#endregion
	}
}