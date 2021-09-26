using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;
using Shop;
using Interact;
using Photon.Pun;
using UnityEngine.UI;
using Menus_UI;

namespace Interact
{
    public class AmmoMachine : Interactable
    {
        [SerializeField] private AmmoType ammoType;
        [SerializeField] private Text ammoTypeText;
        private GoldSystem _goldSystem;
        private MenuManager _menuManager;

        [Range(1, 50)] [SerializeField] private int purchaseAmount;
        [Range(1, 200)] [SerializeField] private int purchasePrice;

        private void Start()
        {
            _goldSystem = GameManager.instance.GetComponent<GoldSystem>();
            _menuManager = MenuManager.Instance.GetComponent<MenuManager>();
            ammoTypeText.text = ammoType.ToString();
        }
        
        public void PurchaseAmmo()
        {
            GameObject customer = GameManager.instance.player;
            //for testing
            _goldSystem.AddGold(new List<string> {PhotonNetwork.NickName}, 10);

            AmmoInventory ammoInventory = customer.gameObject.GetComponent<AmmoInventory>();
            if (ammoInventory == null)
            {
                return;
            }

            ammoInventory.DepositAmmo(ammoType, purchaseAmount);
            _goldSystem.WithdrawGold(PhotonNetwork.NickName, purchasePrice);

            Debug.Log(_goldSystem.GetPlayerGold(PhotonNetwork.NickName));
        }

        public override void Interact(GameObject player)
        {
            _menuManager.OpenMenu("ammoMachine");
        }
    }
}
