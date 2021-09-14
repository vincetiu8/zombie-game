using System;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class GunAttributes : WeaponAttributes
    {
        public int magazineSize;
        public float reloadTime;
        public float bulletVelocity;

        public override string ToString()
        {
            return base.ToString()
                   + $"Magazine Size: {magazineSize}\n"
                   + $"Reload Time: {reloadTime}\n"
                   + $"bulletVelocity: {bulletVelocity}\n";
        }
    }
}