using UnityEngine;
using Weapons;
using Shop;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using Menus_UI;
using Networking;

namespace Interact
{
    ///<summary>
    ///     Handles ammo machine interactions
    ///</summary>

    public class AmmoMachine : Interactable
    {
        
        [SerializeField] private AmmoType ammoType;
        [Header("UI Objects")]
        [SerializeField] private Text ammoTypeText;
        [SerializeField] private Slider purchaseAmountInput;
        [SerializeField] private Text purchaseSliderValue;
        [SerializeField] private Text priceRatioText;
        [SerializeField] private Text totalPurchaseAmount;
        [SerializeField] private GameObject errorText;
        [SerializeField] private GameObject sliderContainer;
        [SerializeField] private GameObject buttonContainer;
        

        private GoldSystem _goldSystem;
        private MenuManager _menuManager;

        private int purchaseAmount;
        private int purchasePrice;
        [Range(1, 10)] [SerializeField] private int purchaseAmountMultiplier;

        private void Start()
        {
            _menuManager = MenuManager.instance.GetComponent<MenuManager>();
            ammoTypeText.text = ammoType.ToString();
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

            CheckIfPlayerHasEnoughMoney();
            Debug.Log(GameManager.instance.goldSystem.GetPlayerGold(PhotonNetwork.LocalPlayer.GetPlayerNumber()));
        }

        public override void Interact(GameObject player)
        {  
            CheckIfPlayerHasEnoughMoney();

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
        private void CheckIfPlayerHasEnoughMoney()
        {
            purchaseAmountInput.minValue = 1;
            purchaseAmountInput.maxValue = (GameManager.instance.goldSystem.GetPlayerGold(PhotonNetwork.LocalPlayer.GetPlayerNumber())) / purchaseAmountMultiplier;

            if(purchaseAmountInput.maxValue <= 0f)
            {
                sliderContainer.SetActive(false);
                buttonContainer.SetActive(false);
                errorText.SetActive(true);
            }
            else
            {
                sliderContainer.SetActive(true);
                buttonContainer.SetActive(true);
                errorText.SetActive(false);
            }
        }
    }
}
