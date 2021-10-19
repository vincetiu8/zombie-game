using Menus_UI;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menus
{
	/// <summary>
	///     Handles opening and closing menus.
	/// </summary>
	public class MenuManager : MonoBehaviour
	{
		public static MenuManager instance;

		[SerializeField] private Menu[]      menus;
		private                  PlayerInput _playerInput;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			if (_playerInput == null)
			{
				// Still causes an error in main menu, that's fine
				_playerInput = GameManager.Instance.localPlayerInstance.GetComponent<PlayerInput>();
			}
		}

		public void OpenMenu(string menuName)
		{
			foreach (Menu menu in menus)
			{
				menu.Toggle(menu.menuName == menuName);
				if (menu.menuName == menuName && menu.freezeOnToggle)
					_playerInput.SwitchCurrentActionMap(menu.inputToggle ? "UI" : "Game");
			}
		}

		public void OpenMenu(Menu menu)
		{
			OpenMenu(menu.menuName);
		}
	}
}