using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;
using Shop;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using Menus_UI;
using Networking;

namespace Interact
{
    public class AmmoMachine : Interactable
    {
        
        [SerializeField] private AmmoType ammoType;
        [Header("UI Objects")]
        [SerializeField] private Text ammoTypeText;
        [SerializeField] private Slider purchaseAmountInput;
        [SerializeField] private Text purchaseSliderValue;
        [SerializeField] private Text priceRatioText;
        [SerializeField] private Text totalPurchaseAmount;

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

        //updates amount in PurchaseSlidervalue text element
        public void SetPurchaseAmount(float value)
        {
            purchaseAmount = Mathf.FloorToInt(purchaseAmountInput.value);
            purchasePrice = purchaseAmount * purchaseAmountMultiplier;

            purchaseSliderValue.text = Mathf.FloorToInt(value).ToString();
            totalPurchaseAmount.text = purchasePrice.ToString();
        }
        
        public void PurchaseAmmo()
        {
            GameObject customer = GameManager.instance.localPlayer;

            AmmoInventory ammoInventory = customer.gameObject.GetComponent<AmmoInventory>();
            if (ammoInventory == null)
            {
                return;
            }

            if(GameManager.instance.goldSystem.WithdrawPlayerGold(purchasePrice))
            {
                ammoInventory.DepositAmmo(ammoType, purchaseAmount);
            }

            Debug.Log(GameManager.instance.goldSystem.GetPlayerGold(PhotonNetwork.LocalPlayer.GetPlayerNumber()));
        }

        public override void Interact(GameObject player)
        {   
            // Set slider values
            purchaseAmountInput.minValue = 0;
            purchaseAmountInput.maxValue = (GameManager.instance.goldSystem.GetPlayerGold(PhotonNetwork.LocalPlayer.GetPlayerNumber())) / purchaseAmountMultiplier;
            
            priceRatioText.text = "";
            priceRatioText.text = purchaseAmountMultiplier.ToString();

            _menuManager.OpenMenu("ammomachine");
        }
    }
}
