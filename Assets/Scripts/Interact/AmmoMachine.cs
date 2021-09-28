using UnityEngine;
using Weapons;
using Shop;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using Menus_UI;
using Networking;
using System.ComponentModel;

namespace Interact
{
    ///<summary>
    ///     Handles ammo machine interactions
    ///</summary>

    public class AmmoMachine : Interactable
    {
        [SerializeField] private AmmoType ammoType;
        
        [Header("Purchase Settings")]
        [Range(1, 10)] [SerializeField] [Description("The amount purchase quantity is multiplied by to get the total price")] 
        private int purchaseAmountMultiplier;

        [Header("UI Objects")]
        [SerializeField] private Text ammoTypeText;
        [SerializeField] private Slider purchaseAmountInput;
        [SerializeField] private Text purchaseSliderValue;
        [SerializeField] private Text priceRatioText;
        [SerializeField] private Text totalPurchaseAmount;
        [SerializeField] private GameObject errorText;
        [SerializeField] private GameObject sliderContainer;
        [SerializeField] private GameObject buttonContainer;

        private MenuManager _menuManager;

        private int purchaseAmount;
        private int purchasePrice;

        private int player;
        private int playerGold;
        private int sliderMax;

        private void Start()
        {
            _menuManager = MenuManager.instance.GetComponent<MenuManager>();
            ammoTypeText.text = ammoType.ToString();

            purchaseAmountInput.minValue = 1;
        }
        
        public void PurchaseAmmo()
        {
            GameObject customer = GameManager.instance.localPlayer;

            AmmoInventory ammoInventory = customer.gameObject.GetComponent<AmmoInventory>();
            if (ammoInventory == null)
            {
                return;
            }

            // Only deposit ammo if player can afford it
            if(GameManager.instance.goldSystem.WithdrawPlayerGold(purchasePrice))
            {
                ammoInventory.DepositAmmo(ammoType, purchaseAmount);
            }

            CheckPlayerSufficientMoney();
        }

        public override void Interact(GameObject player)
        {  
            CheckPlayerSufficientMoney();

            priceRatioText.text = purchaseAmountMultiplier.ToString();

            _menuManager.OpenMenu("ammomachine");
        }

        // Updates amount in PurchaseSlidervalue text element
        public void SetPurchaseAmount(float value)
        {
            purchaseAmount = Mathf.FloorToInt(value);
            purchasePrice = purchaseAmount * purchaseAmountMultiplier;

            purchaseSliderValue.text = Mathf.FloorToInt(value).ToString();
            totalPurchaseAmount.text = purchasePrice.ToString();
        }

        // Calculates player's max amount of ammo purchaseable
        // Toggles on/off slider accordingly
        private void CheckPlayerSufficientMoney()
        {
            player = PhotonNetwork.LocalPlayer.GetPlayerNumber();

            playerGold = GameManager.instance.goldSystem.GetPlayerGold(player);

            sliderMax = playerGold / purchaseAmountMultiplier;

            purchaseAmountInput.maxValue = sliderMax;

            bool showError = purchaseAmountInput.maxValue <= 0;

            sliderContainer.SetActive(!showError);
            buttonContainer.SetActive(!showError);
            errorText.SetActive(showError);
        }
    }
}