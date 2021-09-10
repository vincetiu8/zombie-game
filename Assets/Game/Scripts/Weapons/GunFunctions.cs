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
            public float ammo; // the TOTAL amount of ammo weapon has, ie. if it's 0 your gun can shoot no more bullets
            public float magazineSize; //how many rounds weapon has after a reload
            public float fireRate;
            public float reload; //how many seconds it takes to reload weapon

            public WeaponAttributes(float dmg, float amo, float magsiz, float frate, float re)
            {
                damage = dmg;
                ammo = amo;
                magazineSize = magsiz;
                fireRate = frate;
                reload = re;
            }

            public void Display() //Just for easy debugging
            {
                string result = "Damage: " + damage.ToString() + ", Ammo: " + ammo.ToString() + ", Mag Size: " + ammo.ToString() + ", Fire Rate: " + fireRate.ToString() + ", Reload Time: " + reload.ToString();
                Debug.Log(result);
            }
        }

        
    }
}