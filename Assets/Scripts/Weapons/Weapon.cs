using System;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviourPun
    {
        [SerializeField] protected string weaponName;

        protected int maxLevel;
        protected int currentLevel;
        protected WeaponAttributes currentAttributes;

        private bool _isFiring;
        private float _fireCooldown;

        protected virtual void Start()
        {
            currentLevel = 0;
            maxLevel = 0;
        }

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
            _fireCooldown = currentAttributes.fireCooldown;
        }

        public virtual void Reload()
        {
            
        }

        public virtual void Upgrade()
        {
            // Block upgrading the weapon if there are no more levels left to unlock
            if (currentLevel > maxLevel - 2)
            {
                return;
            }

            currentLevel++;
        }

        public virtual void FaceMouse(float distance)
        {
            
        }
    }
}