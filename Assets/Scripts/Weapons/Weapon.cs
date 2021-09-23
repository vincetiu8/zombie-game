using System;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviourPun
    {
        [Description("The weapon's name")]
        [SerializeField] protected string weaponName;

        // The maximum level of the weapon
        // Should be set in a subclass
        protected int maxLevel;
        
        // The current level of the weapon
        protected int currentLevel;
        
        // The weapon attributes
        protected WeaponAttributes currentAttributes;

        // The player's ammo inventory
        protected AmmoInventory ammoInventory;
        
        // Represents whether the weapon is currently firing for full auto weapons
        // Not used for semi auto
        private bool _isFiring;
        
        // Amount of seconds before the weapon can be fired again
        private float _fireCooldown;

        // Will be false if player is in animation or doing something that shouldn't allow them to fire their weapon
        internal bool CanAttack;

        // Should be overridden to set maxLevel
        protected virtual void Start()
        {
            currentLevel = 0;
            maxLevel = 0;
            CanAttack = true;
        }

        public void Setup(AmmoInventory inventory)
        {
            ammoInventory = inventory;
        }

        private void Update()
        {
            // Only calculate updates for local player
            if (!photonView.IsMine) return;
            
            // Decrease _fireCooldown accordingly
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
        
        // Called when mouse is pressed or released
        // Toggle is true when pressed, false when released
        public void ToggleFire(bool toggle)
        {
            if (!CanAttack) return;
            if (!currentAttributes.fullAuto)
            {
                // Just fires when pressed for semi auto
                if (toggle && _fireCooldown <= 0)
                {
                    Fire();
                }
                
                return;
            }

            // Toggles firing for full auto weapons
            _isFiring = toggle;
        }

        // Base fire method, should be overridden by subclasses
        protected virtual void Fire()
        {
            _fireCooldown = currentAttributes.fireCooldown;
        }

        // Base reload method, should be overridden by subclasses
        public virtual void Reload()
        {
            
        }

        // Base upgrade method, should be overridden by subclasses
        public virtual void Upgrade()
        {
            // Block upgrading the weapon if there are no more levels left to unlock
            if (currentLevel > maxLevel - 2)
            {
                return;
            }

            currentLevel++;
        }

        // Optional method to align weapon with cursor if needed
        public virtual void FaceMouse(float distance)
        {
            
        }
    }
}