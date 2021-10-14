using System;
using System.Security.Cryptography;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    /// <summary>
    /// Used for things you would realistically unlock only once, such as armor, perks, weapons, etc
    /// </summary>
    public class Unlockable : Shop
    {
        [SerializeField] protected string itemName;

        [SerializeField] protected bool _hasSprite;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Sprite beforeUnlock;
        [SerializeField] protected Sprite afterUnlock;

        private bool _isUnlocked;
        
        protected override void Start()
        {
            base.Start();
            
            if (!_hasSprite) return;
            spriteRenderer.sprite = beforeUnlock;
        }

        protected override string GetShopPrompt()
        {
            return $"{itemName}?";
        }

        protected override void OnPurchase()
        {
            Unlock();
        }

        protected virtual void Unlock()
        {
            if (_isUnlocked) return;
            _isUnlocked = true;
            
            if (!_hasSprite) return;
            spriteRenderer.sprite = afterUnlock;
        }
        
        /// <summary>
        /// Called when a reset event occurs
        /// Example: A player losing their perks when they die.
        /// </summary>
        public virtual void Relock()
        {
            _isUnlocked = false;
            
            if (!_hasSprite) return;
            spriteRenderer.sprite = beforeUnlock;
        }
  
    }
}