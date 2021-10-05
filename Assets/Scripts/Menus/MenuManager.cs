using Networking;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Menus_UI
{
	/// <summary>
	///     Handles opening and closing menus.
	/// </summary>
	// MenuManager 
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
				_playerInput = GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>();
			}
		}

		public void OpenMenu(string menuName)
		{
			foreach (Menu menu in menus)
			{
				menu.Toggle(menu.menuName == menuName);
				if (menu.freezeOnToggle)
				{
				}
                Debug.Log(GameManager.instance.localPlayerInstance);
					MiscUtils.ToggleInput(menu.inputToggle, _playerInput);
                    //	Utils.ToggleInput(menu.inputToggle ? Utils.ActionMapOptions.UI : Utils.ActionMapOptions.Game, _playerInput);


			}
		}

		public void OpenMenu(Menu menu)
		{
			OpenMenu(menu.menuName);
		}
	}
}