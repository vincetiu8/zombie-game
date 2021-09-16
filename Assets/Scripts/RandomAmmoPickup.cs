using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class RandomAmmoPickup : AmmoPickup
{
    [SerializeField] private AmmoType _ammoType;
    [Range(1, 50)] [SerializeField] private int _minDrop;
    [Range(1, 50)] [SerializeField] private int _maxDrop;

    //this one lets the ammo prefab it's attached to drop a random amount
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        AmmoInventory _ammoInventory = collision.gameObject.GetComponent<AmmoInventory>();
        if(_ammoInventory == null)
        {
            return;
        }
        Destroy(this.gameObject);
        _ammoInventory.DepositAmmo(_ammoType, Random.Range(_minDrop, _maxDrop));
    }
}
