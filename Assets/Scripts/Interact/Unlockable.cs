using System;
using System.Security.Cryptography;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Interact
{
    /// <summary>
    /// Used for things you would realistically unlock only once, such as armor, perks, weapons, etc
    /// </summary>
    public class Unlockable : Interactable
    {
        [SerializeField] private string itemName;
        [SerializeField] private int purchasePrice;

        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite beforeUnlock;
        [SerializeField] private Sprite afterUnlock;

        [SerializeField] protected Canvas canvas;
        [SerializeField] private Text itemNameUI;
        [SerializeField] private Text itemPriceUI;

        private bool _isUnlocked;
        
        protected override void Start()
        {
            base.Start();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = beforeUnlock;
            itemNameUI.text = itemName;
            itemPriceUI.text = purchasePrice.ToString();
        }

        public override void Interact()
        {

            if (GameManager.instance.goldSystem.WithdrawPlayerGold(purchasePrice))
            {
                Unlock();
                return;
            }
            // Is an exception to prevent code from further running in overridden methods
            throw new Exception("Player does not have enough money");
        }

        protected virtual void Unlock()
        {
            if (_isUnlocked) return;
            _spriteRenderer.sprite = afterUnlock;
            _isUnlocked = true;
        }
        
        /// <summary>
        /// Called when a reset event occurs
        /// Example: A player losing their perks when they die.
        /// </summary>
        public virtual void Relock()
        {
            _spriteRenderer.sprite = beforeUnlock;
            _isUnlocked = false;
        }
  
    }
}