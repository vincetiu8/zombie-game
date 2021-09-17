using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Collectibles
{
    public class AmmoPickup : Collectible
    {
        [SerializeField] private AmmoType ammoType;
        [SerializeField] protected int dropAmount;

        protected override void Pickup(GameObject player)
        {
            AmmoInventory ammoInventory = player.gameObject.GetComponent<AmmoInventory>();
            if (ammoInventory == null)
            {
                return;
            }

            Debug.Log(dropAmount);
            ammoInventory.DepositAmmo(ammoType, dropAmount);
        }
    }
}