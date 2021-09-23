using UnityEngine;

namespace Networking
{
	public class MenuManager : MonoBehaviour
	{
		#region Variables
		public static MenuManager Instance;

		[SerializeField] Menu[] menus;
		#endregion

		#region Unity Methods
		void Awake()
		{
			Instance = this;
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
		}
		#endregion
	}
}