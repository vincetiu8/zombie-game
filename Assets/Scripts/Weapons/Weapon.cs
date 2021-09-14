using UnityEngine;

namespace Weapons
{
    public interface IWeapon
    {
        public void Setup(AmmoInventory ammoInventory);
        
        public void ToggleFire(bool isFiring);

        public void Reload();

        public void Upgrade();
    }
}