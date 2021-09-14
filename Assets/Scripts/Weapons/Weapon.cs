using UnityEngine;

namespace Weapons
{
    public interface IWeapon
    {
        public void Setup(AmmoInventory ammoInventory);
        
        public void Fire();

        public void Reload();
    }
}