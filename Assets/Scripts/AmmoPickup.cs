using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoType _ammoType;
    [SerializeField] private int _dropAmount;

    //this one lets the ammo prefab it's attached to drop a specific amount
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {   
            AmmoInventory _ammoInventory = collision.gameObject.GetComponent<AmmoInventory>();
            if(_ammoInventory == null)
            {
                return;
            }
            Destroy(this.gameObject);
            _ammoInventory.DepositAmmo(_ammoType, _dropAmount);
        }   
    }
}
