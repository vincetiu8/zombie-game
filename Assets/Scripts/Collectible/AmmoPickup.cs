using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class AmmoPickup : Collectible
{
    [SerializeField] private AmmoType _ammoType;
    [SerializeField] protected int _dropAmount;

    protected override void Pickup(GameObject _player)
    {
        AmmoInventory _ammoInventory = _player.gameObject.GetComponent<AmmoInventory>();
        if (_ammoInventory == null)
        {
            return;
        }

        Debug.Log(_dropAmount);
        _ammoInventory.DepositAmmo(_ammoType, _dropAmount);
    }
}