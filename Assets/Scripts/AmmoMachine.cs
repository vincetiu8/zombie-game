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
using UnityEngine.InputSystem;

public class AmmoMachine : Interactable
{

    #region private variables

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private Text ammoTypeText;
    [SerializeField] private GameObject ammoMachineUI;
    private GoldSystem goldSystem;
    private MenuManager menuManager;
    private GameObject customer;

    #endregion


    #region private methods

    private void Start()
    {
        goldSystem = GameManager.instance.GetComponent<GoldSystem>();
        menuManager = MenuManager.Instance.GetComponent<MenuManager>();
        ammoTypeText.text = ammoType.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
            customer = collision.gameObject;
        }
    }

    #endregion


    #region public methods

    public void PurchaseAmmo(GameObject _customer)
    {
        _customer = customer;
        //for testing
        goldSystem.AddGold(new List<string> {PhotonNetwork.NickName}, 10);

        AmmoInventory ammoInventory = _customer.gameObject.GetComponent<AmmoInventory>();
        if(ammoInventory == null)
        {
            return;
        }
        ammoInventory.DepositAmmo(ammoType, 10);
        goldSystem.WithdrawGold(PhotonNetwork.NickName, 5);

        Debug.Log(goldSystem.GetPlayerGold(PhotonNetwork.NickName));
    }

    public override void Interact(GameObject player)
    {
        menuManager.OpenMenu("ammoMachine");
    }

    #endregion
}
