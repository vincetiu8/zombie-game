using UnityEngine;

namespace Objects
{
	public class GoldPopup : Popup
	{
		public void Setup(int goldChangeAmount)
		{
			TextColor = Color.yellow;
			if (goldChangeAmount > 0)
				TextMeshPro.SetText("+" + goldChangeAmount);
			else
				TextMeshPro.SetText(goldChangeAmount.ToString());

			base.Setup();
		}
	}
}