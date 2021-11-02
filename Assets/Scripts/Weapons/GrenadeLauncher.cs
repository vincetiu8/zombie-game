using System;
using UnityEngine;

namespace Weapons
{
    
    public class GrenadeLauncher : ProjectileLauncher
    {
        private void Update()
        {
            _bulletsInMagazine = ammoInventory.GetAmmo(ammoType);
        }

        // Lets the grenade launcher bypass reloading
        protected override void FireBullet(float angle)
        {
            base.FireBullet(angle);
            ammoInventory.WithdrawAmmo(ammoType, 1);
        }
    }
}
