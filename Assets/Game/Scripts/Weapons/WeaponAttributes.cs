using UnityEngine;
using System;

namespace Game.Scripts.Weapons
{
    [Serializable]
    public struct WeaponAttributes
    {
        public string description;
        public float damage;
        public int magazineSize;
        public float fireCooldown;
        public float reloadTime;
        public float bulletVelocity;
        public GameObject bulletPrefab;

        public WeaponAttributes(
            string description, 
            float damage, 
            int magazineSize, 
            float fireCooldown,
            float reloadTime,
            float bulletVelocity, 
            GameObject bulletPrefab
            )
        {
            this.description = description;
            this.damage = damage;
            this.magazineSize = magazineSize;
            this.fireCooldown = fireCooldown;
            this.reloadTime = reloadTime;
            this.bulletVelocity = bulletVelocity;
            this.bulletPrefab = bulletPrefab;
        }

        public override string ToString()
        {
            return $"Magazine Size: {magazineSize}\n"
                   + $"Fire Cooldown: {fireCooldown}\n"
                   + $"Reload Time: {reloadTime}\n"
                   + $"Bullet Velocity: {bulletVelocity}\n";
        }
    }
}