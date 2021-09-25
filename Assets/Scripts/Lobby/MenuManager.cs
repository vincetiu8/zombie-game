using UnityEngine;

namespace Lobby
{
	// MenuManager handles opening and closing menus
	public class MenuManager : MonoBehaviour
	{
		#region Unity Methods

		private void Awake()
		{
			instance = this;
		}

		#endregion

		#region Variables

		public static MenuManager instance;

		[SerializeField] private Menu[] menus;

		#endregion

		#region Public Methods

		public void OpenMenu(string menuName)
		{
			foreach (Menu menu in menus) menu.Toggle(menu.menuName == menuName);
		}

		public void OpenMenu(Menu menu)
		{
			OpenMenu(menu.menuName);
		}

		#endregion
	}
}