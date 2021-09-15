using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoType _ammoType;
    private int _dropAmount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {   
            _dropAmount = Random.Range(1, 10);
            Destroy(this.gameObject);
            AmmoInventory _ammoInventory = collision.gameObject.GetComponent<AmmoInventory>();
            _ammoInventory.DepositAmmo(_ammoType, _dropAmount);
        }   
    }
}
