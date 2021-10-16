using System;
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

		[SerializeField] [Range(0.1f, 2f)] private float popupDelay = 1f;

		private float _popupTimeRemaining;

		protected override void Start()
		{
			base.Start();

			GameManager.Instance.goldSystem.playerGoldChanged.AddListener(UpdateShopStatus);
		}

		protected override void Update()
		{
			base.Update();

			if (_popupTimeRemaining > 0)
			{
				_popupTimeRemaining -= Time.deltaTime;
				return;
			}
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            ShopText.Instance.ToggleVisibility(false);
        }

        protected abstract string GetShopPrompt();

		/// <summary>
		///     Used as the listener when player gold changes.
		///     We can't return a value for listener functions.
		/// </summary>
		private void UpdateShopStatus()
		{
            if (!LocallyInteracting) return;
			SetShopStatus();
		}

		private bool SetShopStatus()
		{
			int gold = GameManager.Instance.goldSystem.GetPlayerGold();
			bool canBuy = gold >= itemCost;
			ShopText.Instance.SetText(GetShopPrompt() + $" ({itemCost})", canBuy ? Color.white : Color.red);
			return canBuy;
		}

		public override void StartInteraction()
		{
			ShopText.Instance.ToggleVisibility(true);

			// We only want to let the player start if they have already seen the popup and have enough money
			if (SetShopStatus() && _popupTimeRemaining > 0)
			{
				base.StartInteraction();
			}

			_popupTimeRemaining = popupDelay;
		}

		protected override void FinishInteraction()
		{
			if (GameManager.Instance.goldSystem.WithdrawPlayerGold(itemCost))
			{
				OnPurchase();
			}

			_popupTimeRemaining = popupDelay;
			if (!SetShopStatus()) ShopText.Instance.ToggleVisibility(false);
			base.FinishInteraction();
		}

		protected abstract void OnPurchase();
	}
}