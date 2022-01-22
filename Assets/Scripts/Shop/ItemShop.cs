namespace Shop
{
	public abstract class ItemShop : Shop
	{
		protected override string GetShopPrompt()
		{
			return $"Buy {itemName}?";
		}
	}
}