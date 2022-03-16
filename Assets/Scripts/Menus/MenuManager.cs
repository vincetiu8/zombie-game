using UnityEngine;

namespace Menus
{
	/// <summary>
	///     Handles opening and closing menus.
	/// </summary>
	public class MenuManager : MonoBehaviour
	{
		public static MenuManager Instance;

		[SerializeField] private MenuController initialMenu;

		private MenuController _currentMenu;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			OpenMenu(initialMenu);
		}

		public void OpenMenu(MenuController menuControllerToOpen)
		{
			if (_currentMenu != null) _currentMenu.Toggle(false);

			_currentMenu = menuControllerToOpen;

			if (_currentMenu != null) _currentMenu.Toggle(true);
		}
	}
}