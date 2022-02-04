using UnityEngine;

namespace Menus
{
	public class CreateRoomMenu : Menu
	{
		[SerializeField] private GameObject errorText;

		public override void Toggle(bool active)
		{
			base.Toggle(active);
			errorText.SetActive(false);
		}
	}
}