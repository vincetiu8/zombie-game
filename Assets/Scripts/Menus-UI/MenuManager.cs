using UnityEngine;
using UnityEngine.InputSystem;

namespace Menus_UI
{
	public class MenuManager : MonoBehaviour
	{
		#region Variables
		public static MenuManager Instance;
		[SerializeField] Menu[] menus;
		private PlayerInput playerInput;
		#endregion

		#region Unity Methods
		void Awake()
		{
			Instance = this;
		}

		void Start()
		{
			//this is gonna throw a nullreference in the start menu
			//that's fine and intentional, so we can keep this in one script
			if(playerInput == null)
			{
				playerInput = GameManager.instance.player.GetComponent<PlayerInput>();
			}
		}
		#endregion

		#region Public Methods
		public void OpenMenu(string menuName)
		{
			for(int i = 0; i < menus.Length; i++)
			{
				if(menus[i].menuName == menuName)
				{
					menus[i].Open();
					if (menus[i].isFreezable)
					{
						Utils.DisableInput(!menus[i].isFreezable, playerInput);
					}
				}
				else if(menus[i].open)
				{
					CloseMenu(menus[i]);
				}
			}
		}

		public void OpenMenu(Menu menu)
		{
			OpenMenu(menu.menuName);
		}

		public void CloseMenu(Menu menu)
		{
			menu.Close();
			if(!menu.isFreezable) return;
			Utils.DisableInput(menu.isFreezable, playerInput);
		}
		#endregion
	}
}