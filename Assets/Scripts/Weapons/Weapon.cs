using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviourPun
    {
        [SerializeField] protected string weaponName;

        protected int currentLevel;
        protected WeaponAttributes currentAttributes;

        private bool _isFiring;
        private float _fireCooldown;

        private void Update()
        {
            if (!photonView.IsMine) return;
            
            if (_fireCooldown > 0)
            {
                _fireCooldown -= Time.deltaTime;
                return;
            }

            // Check whether we can fire here again
            if (currentAttributes.fullAuto && _isFiring)
            {
                Fire();
            } 
        }
        
        public void ToggleFire(bool toggle)
        {
            if (!currentAttributes.fullAuto)
            {
                if (toggle && _fireCooldown <= 0)
                {
                    Fire();
                }
                
                return;
            }

            _isFiring = toggle;
        }

        protected virtual void Fire()
        {
            Debug.Log("Firing!");
            _fireCooldown = currentAttributes.fireCooldown;
        }

        public virtual void Reload() { }

        public abstract void Upgrade();
    }
}