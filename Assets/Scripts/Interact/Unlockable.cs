using System;
using System.Security.Cryptography;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Interact
{/*
    /// <summary>
    /// Used for things you would realistically unlock only once, such as armor, perks, weapons, etc
    /// </summary>
    public class Unlockable : Interactable
    {
        [SerializeField] protected string itemName;
        [SerializeField] protected int purchasePrice;

        [SerializeField] protected bool _hasSprite;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Sprite beforeUnlock;
        [SerializeField] protected Sprite afterUnlock;

        [SerializeField] protected bool _hasCanvas;
        [SerializeField] protected Canvas myCanvas;
        [SerializeField] protected Text itemNameUI;
        [SerializeField] protected Text itemPriceUI;

        private bool _isUnlocked;
        
        protected override void Start()
        {
            base.Start();
            itemNameUI.text = itemName;
            itemPriceUI.text = purchasePrice.ToString();
            
            if (!_hasSprite) return;
            spriteRenderer.sprite = beforeUnlock;
        }

        public override void Interact()
        {
            if (!GameManager.instance.goldSystem.WithdrawPlayerGold(purchasePrice)) return;
            EnoughMoneyInteract();
        }
        
        protected virtual void EnoughMoneyInteract()
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
  
    }*/
}