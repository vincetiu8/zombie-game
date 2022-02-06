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

		[SerializeField] protected string itemName;

		[SerializeField] [Range(0.1f, 2f)] private float popupDelay = 1f;
		private                                    bool  _popupActive;

		private float _popupTime;

		protected override void Start()
		{
			base.Start();

			GameManager.Instance.goldSystem.playerGoldChanged.AddListener(UpdateShopStatus);
		}

		protected override void Update()
		{
			base.Update();

			if (!_popupActive) return;

			if (_popupTime > 0)
			{
				_popupTime -= Time.deltaTime;
				return;
			}

			ToggleText(false);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Player")) return;
			ToggleText(false);
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

		protected virtual bool SetShopStatus()
		{
			bool canBuy = CanBuy();
			ShopText.Instance.SetText(GetShopPrompt() + $" ({itemCost})", canBuy ? Color.white : Color.red);
			return canBuy;
		}

		protected virtual bool CanBuy()
		{
			int gold = GameManager.Instance.goldSystem.GetPlayerGold();

			return gold >= itemCost;
		}

		private void ToggleText(bool toggle)
		{
			_popupActive = toggle;
			ShopText.Instance.ToggleVisibility(toggle);
		}

		public override void StartInteraction()
		{
			ResetDuration();
			ToggleText(true);

			// We only want to let the player start if they have already seen the popup and have enough money
			if (SetShopStatus() && _popupTime > 0)
			{
				Debug.Log("starting purchase interaction");

				base.StartInteraction();
			}

			_popupTime = popupDelay;
		}

		protected override void OnSuccessfulInteraction()
		{
			if (GameManager.Instance.goldSystem.WithdrawPlayerGold(itemCost))
			{
				OnPurchase();
			}

			_popupTime = popupDelay;
			if (!SetShopStatus()) ToggleText(false);
			FinishInteraction();
		}

		protected abstract void OnPurchase();
	}
}