using Photon.Pun;

namespace Shop
{
	/// <summary>
	///     Used for things you would realistically unlock only once, such as armor, perks, weapons, etc
	/// </summary>
	public abstract class Unlockable : Shop
	{
		private bool _isUnlocked;

		protected override string GetShopPrompt()
		{
			return $"Unlock {itemName}?";
		}

		protected override void OnPurchase()
		{
			if (_isUnlocked) return;
			photonView.RPC("ToggleUnlock", RpcTarget.All, true);
		}

		[PunRPC]
		protected virtual void ToggleUnlock(bool toggle)
		{
			_isUnlocked = toggle;
		}
	}
}