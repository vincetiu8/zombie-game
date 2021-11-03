using System;
using UnityEngine;

namespace Weapons
{
    // Lets the throwable grenade bypass reloading
    public class ThrowableGrenade : ProjectileLauncher
    {
        private void Update()
        {
            _currentGunAttributes.magazineSize = ammoInventory.GetAmmo(ammoType);
            _bulletsInMagazine = ammoInventory.GetAmmo(ammoType);
        }
        
        protected override void FireBullet(float angle)
        {
            base.FireBullet(angle);
            ammoInventory.WithdrawAmmo(ammoType, 1);
        }
    }
}
