using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Weapons
{
    public class GunFunctions : MonoBehaviour
    {
        public class WeaponAttributes
        {
            public float damage;
            public float maxAmmo; // the TOTAL amount of ammo weapon has, ie. if it's 0 your gun can shoot no more bullets
            public float magazineSize; //how many rounds weapon has after a reload
            public float fireCooldown;
            public float reload; //how many seconds it takes to reload weapon

            public WeaponAttributes(float dmg, float amo, float magsiz, float frcd, float re)
            {
                damage = dmg;
                maxAmmo = amo;
                magazineSize = magsiz;
                fireCooldown = frcd;
                reload = re;
            }

            public void Display() //Just for easy debugging
            {
                string result = "Damage: " + damage.ToString() + ", Ammo: " + maxAmmo.ToString() + ", Mag Size: " + magazineSize.ToString() + ", Fire Rate: " + fireCooldown.ToString() + ", Reload Time: " + reload.ToString();
                Debug.Log(result);
            }
        }

        /*
        public void AddWeaponTier(List<WeaponAttributes> weaponAttributes,float damage, float ammo, float magazineSize, float fireRate, float reload)
        {
            weaponAttributes.Add(new GunFunctions.WeaponAttributes(damage, ammo, magazineSize, fireRate, reload));
        }*/

    }
}