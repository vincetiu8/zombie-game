using UnityEngine;

namespace Menus
{
	public class CreateRoomMenuController : MenuController
	{
		[SerializeField] private GameObject errorText;

		public override void Toggle(bool active)
		{
			base.Toggle(active);
			errorText.SetActive(false);
		}
	}
}