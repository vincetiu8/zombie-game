using UnityEngine;

namespace Menus
{
	/// <summary>
	///     Handles each menu.
	///     Provides functionality to toggle them on or off.
	/// </summary>
	public class MenuController : MonoBehaviour
	{
		public virtual void Toggle(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}