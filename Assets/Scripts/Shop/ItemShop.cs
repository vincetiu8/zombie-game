using UnityEngine;

namespace Shop
{
	public abstract class ItemShop : Shop
	{
		[Header("Item Shop Settings")] [SerializeField]
		private string itemName;

		protected override string GetShopPrompt()
		{
			return $"Buy {itemName}?";
		}
	}
}