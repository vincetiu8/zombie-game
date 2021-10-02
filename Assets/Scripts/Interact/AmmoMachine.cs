using System.ComponentModel;
using Menus_UI;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Interact
{
	/// <summary>
	///     Handles ammo machine interactions
	/// </summary>
	public class AmmoMachine : PressInteractable
	{
		[SerializeField] private AmmoType ammoType;

		[Header("Purchase Settings")]
		[Range(1, 10)]
		[SerializeField]
		[Description("The price of one item, will be multiplied with purchase quantity to get the total price")]
		private int purchaseAmountMultiplier;

		[Header("UI Objects")] [SerializeField]
		private Text ammoTypeText;

		[SerializeField] private Slider     purchaseAmountInput;
		[SerializeField] private Text       purchaseSliderValue;
		[SerializeField] private Text       priceRatioText;
		[SerializeField] private Text       totalPurchaseAmount;
		[SerializeField] private GameObject errorText;
		[SerializeField] private GameObject sliderContainer;
		[SerializeField] private GameObject buttonContainer;

		private MenuManager _menuManager;

		private int _purchaseAmount;
		private int _purchasePrice;

		protected override void Start()
		{
			base.Start();

			_menuManager = MenuManager.instance.GetComponent<MenuManager>();
			ammoTypeText.text = ammoType.ToString();

			purchaseAmountInput.minValue = 1;
		}

		public void PurchaseAmmo()
		{
			GameObject customer = GameManager.instance.localPlayerInstance;

			AmmoInventory ammoInventory = customer.gameObject.GetComponent<AmmoInventory>();
			if (ammoInventory == null) return;

			// Only deposit ammo if player can afford it
			if (GameManager.instance.goldSystem.WithdrawPlayerGold(_purchasePrice))
				ammoInventory.DepositAmmo(ammoType, _purchaseAmount);

			CheckPlayerSufficientMoney();
		}

        protected override void Interact()
		{
			CheckPlayerSufficientMoney();

			priceRatioText.text = purchaseAmountMultiplier.ToString();

			_menuManager.OpenMenu("ammomachine");
		}

		// Updates amount in PurchaseSliderValue text element
		public void SetPurchaseAmount(float value)
		{
			_purchaseAmount = Mathf.FloorToInt(value);
			_purchasePrice = _purchaseAmount * purchaseAmountMultiplier;

			purchaseSliderValue.text = _purchaseAmount.ToString();
			totalPurchaseAmount.text = _purchasePrice.ToString();
		}

		// Calculates player's max amount of ammo purchasable
		// Toggles on/off slider accordingly
		private void CheckPlayerSufficientMoney()
		{
			int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();

			int playerGold = GameManager.instance.goldSystem.GetPlayerGold(playerNumber);

			int sliderMax = playerGold / purchaseAmountMultiplier;

			bool showError = sliderMax <= 0;

			sliderContainer.SetActive(!showError);
			buttonContainer.SetActive(!showError);
			errorText.SetActive(showError);
			if (showError) return;
			purchaseAmountInput.maxValue = sliderMax;
		}
	}
}