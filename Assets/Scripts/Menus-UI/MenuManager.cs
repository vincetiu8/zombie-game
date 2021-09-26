using Lobby;
using UnityEngine;

namespace Menus_UI
{
	/// <summary>
	///     Handles opening and closing menus.
	/// </summary>
	// MenuManager 
	public class MenuManager : MonoBehaviour
	{
		public static MenuManager instance;

		[SerializeField] private Menu[] menus;

		private void Awake()
		{
			instance = this;
		}

		public void OpenMenu(string menuName)
		{
			foreach (Menu menu in menus) menu.Toggle(menu.menuName == menuName);
		}

		public void OpenMenu(Menu menu)
		{
			OpenMenu(menu.menuName);
		}
	}
}