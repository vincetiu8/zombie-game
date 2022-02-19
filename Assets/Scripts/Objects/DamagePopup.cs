using UnityEngine;

namespace Objects
{
	public class DamagePopup : Popup
	{
		public void Setup(int damageAmount)
		{
			if (damageAmount > 0)
			{
				TextColor = Color.green;
				TextMeshPro.SetText("+" + damageAmount);
			}
			else
			{
				TextColor = Color.red;
				TextMeshPro.SetText(damageAmount.ToString());
			}

			base.Setup();
		}
	}
}