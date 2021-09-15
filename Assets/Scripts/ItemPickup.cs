using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private AmmoType _ammoType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            AmmoInventory _ammoInventory = collision.gameObject.GetComponent<AmmoInventory>();
            _ammoInventory.DepositAmmo(_ammoType, 5);
            Debug.Log(_ammoInventory.GetAmmo(_ammoType));
        }   
    }
}
