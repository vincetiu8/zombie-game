using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Weapons
{
    // Lets the throwable grenade bypass reloading
    public class ThrowableGrenade : ProjectileLauncher
    {
        [Tooltip("How much to multiply throw force by when using alt-fire")]
        [SerializeField] private float altFireForceMultiplier;
        
        private void Update()
        {
            _currentGunAttributes.magazineSize = ammoInventory.GetAmmo(ammoType);
            _bulletsInMagazine = ammoInventory.GetAmmo(ammoType);
        }
        
        protected override void FireBullet(float angle, bool alt)
        {
            ammoInventory.WithdrawAmmo(ammoType, 1);
            base.FireBullet(angle, alt);
        }

        protected override void AltFireAction(GameObject bulletClone, float angle)
        {
            SetBulletAttributes(bulletClone, angle, altFireForceMultiplier);
        }
    }
}
