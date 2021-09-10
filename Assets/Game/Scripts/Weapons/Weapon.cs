using System;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;

namespace Game.Scripts.Weapons
{
    // If we ever add melee weapons, we'll have to make a parent class and have this one inherit from it.
    [Serializable]
    public class Weapon
    {
        public string weaponName;
        public int weaponLevel;
        public int bulletsInMagazine;
        public float fireCooldown;
        [SerializeField] private WeaponAttributes[] weaponLevels;

        public void Setup()
        {
            bulletsInMagazine = 0;
        }

        public void Upgrade()
        {
            if (weaponLevel + 1 <= weaponLevels.Length) return;

            weaponLevel += 1;
        }

        public WeaponAttributes GetAttributes()
        {
            return weaponLevels[weaponLevel];
        }

        public void Fire()
        {
            fireCooldown = GetAttributes().fireCooldown;
            bulletsInMagazine--;
        }

        public override string ToString()
        {
            return $"{weaponName} Level {weaponLevel} Stats:\n{GetAttributes()}";
        }
    }
}