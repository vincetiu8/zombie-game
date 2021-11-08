using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

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
        
        protected override void FireBullet(float angle, bool alt)
        {
            ammoInventory.WithdrawAmmo(ammoType, 1);
            base.FireBullet(angle, alt);
        }

        protected override void AltFireAction(GameObject bulletClone, float angle)
        {
            Vector2 direction = TransformUtils.DegToVector2(angle);
            bulletClone.GetComponent<Rigidbody2D>().AddForce(direction * (throwForce * 2), ForceMode2D.Impulse);
            bulletClone.GetComponent<BulletController>().damage = currentAttributes.damage;
        }
    }
}
