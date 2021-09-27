using UnityEngine;
using UnityEngine.InputSystem;
using Networking;

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
		private PlayerInput playerInput;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			if(playerInput == null)
			{
				playerInput = GameManager.instance.localPlayer.GetComponent<PlayerInput>();
			}
		}

		public void OpenMenu(string menuName)
		{
			foreach (Menu menu in menus)
			{
				menu.Toggle(menu.menuName == menuName);
				if(menu.freezeOnToggle)
				{
					Utils.ToggleInput(menu.inputToggle, playerInput);
				}
			}
		}

		public void OpenMenu(Menu menu)
		{
			OpenMenu(menu.menuName);
		}
	}
}