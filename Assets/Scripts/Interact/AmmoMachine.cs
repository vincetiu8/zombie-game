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
        [SerializeField] private Text ammoTypeText;
        [SerializeField] private InputField purchaseAmountInput;
        private GoldSystem _goldSystem;
        private MenuManager _menuManager;

        [Range(1, 50)] private int purchaseAmount;
        [Range(1, 200)] private int purchasePrice;
        [Range(1, 10)] [SerializeField] private int purchaseAmountMultiplier;

        private void Start()
        {
            _menuManager = MenuManager.instance.GetComponent<MenuManager>();
            ammoTypeText.text = ammoType.ToString();
        }
        
        public void PurchaseAmmo()
        {
            GameObject customer = GameManager.instance.localPlayer;

            purchaseAmount = int.Parse(purchaseAmountInput.text);
            purchasePrice = purchaseAmount * purchaseAmountMultiplier;

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
            _menuManager.OpenMenu("ammomachine");
        }
    }
}
