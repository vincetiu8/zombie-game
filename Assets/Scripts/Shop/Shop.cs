using Interact;
using Networking;
using UnityEngine;

namespace Shop
{
	/// <summary>
	///     Base class for all objects that allow a player to purchase something.
	/// </summary>
	public abstract class Shop : Interactable
	{
		[Header("Shop Settings")] [SerializeField]
		private int itemCost;

		protected override void Start()
		{
			base.Start();

			GameManager.instance.goldSystem.playerGoldChanged.AddListener(SetShopText);
		}

		protected abstract string GetShopPrompt();

		public override void OnClosestInteractable()
		{
			SetShopText();
			ShopText.Instance.ToggleVisibility(true);
		}

		public override void OnNotClosestInteractable()
		{
			ShopText.Instance.ToggleVisibility(false);
		}

		private void SetShopText()
		{
			int gold = GameManager.instance.goldSystem.GetPlayerGold();
			ShopText.Instance.SetText(GetShopPrompt() + $" ({itemCost})", gold < itemCost ? Color.red : Color.white);
		}

		public override void Interact()
		{
			if (GameManager.instance.goldSystem.WithdrawPlayerGold(itemCost))
			{
				OnPurchase();
				return;
			}

			SetShopText();
		}

		protected abstract void OnPurchase();
	}
}