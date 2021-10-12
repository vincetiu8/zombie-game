using Interact;
using Networking;
using UnityEngine;

namespace Shop
{
	/// <summary>
	///     Base class for all objects that allow a player to purchase something.
	/// </summary>
	public abstract class Shop : TimedInteractable
	{
		[Header("Shop Settings")] [SerializeField]
		private int itemCost;

		protected override void Start()
		{
			base.Start();

			GameManager.instance.goldSystem.playerGoldChanged.AddListener(UpdateShopStatus);
		}

		protected abstract string GetShopPrompt();

		public override void OnClosestInteractable()
		{
			SetShopStatus();
			ShopText.Instance.ToggleVisibility(true);
		}

		public override void OnNotClosestInteractable()
		{
			ShopText.Instance.ToggleVisibility(false);
		}

		/// <summary>
		///     Used as the listener when player gold changes.
		///     We can't return a value for listener functions.
		/// </summary>
		private void UpdateShopStatus()
		{
			SetShopStatus();
		}

		private bool SetShopStatus()
		{
			int gold = GameManager.instance.goldSystem.GetPlayerGold();
			bool canBuy = gold >= itemCost;
			ShopText.Instance.SetText(GetShopPrompt() + $" ({itemCost})", canBuy ? Color.white : Color.red);
			return canBuy;
		}

		public override void StartInteraction()
		{
			if (!SetShopStatus())
			{
				finishInteraction.Invoke();
				return;
			}

			base.StartInteraction();
		}

		protected override void FinishInteraction()
		{
			if (GameManager.instance.goldSystem.WithdrawPlayerGold(itemCost))
			{
				OnPurchase();
			}

			SetShopStatus();
			base.FinishInteraction();
		}

		protected abstract void OnPurchase();
	}
}