using System;
using System.Linq;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interact
{
	/// <summary>
	///     Interactable is the base class for all interactable objects.
	///     It handles players entering and exiting the intractable trigger area.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public abstract class Interactable : MonoBehaviourPun
    {
        protected bool _cancelledAlready = true;
        // Added for debugging convenience
		protected virtual void Start()
		{
            Collider2D[] cols = GetComponents<Collider2D>();
			if (!cols.Any(col => col.isTrigger))
				Debug.LogError("No trigger colliders attached to interactable object, can't interact");
		}

		protected virtual void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
				collision.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
		}

		protected virtual void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
				collision.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
		}

		/// <summary>
		///     Callback when the player interacts with an object.
		///     We don't pass in the player, but it can be assumed that the interacting player is the local player.
		/// </summary>
		public abstract void Interact(bool contextPerformed);

        public virtual void CancelInteraction()
        {
            
        }
    }
    
    public abstract class HoldInteractable : Interactable
    {
        private                  PlayerInput _playerInput;
        
        protected override void Start()
        {
            base.Start();
                 
        }

        public override void Interact(bool contextPerformed)
        {
            if (_playerInput == null)
            {
                _playerInput = GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>();
            }  
           // (contextPerformed ? (Action)StartInteraction : CancelInteraction)();
           StartInteraction();
        }

        protected virtual void StartInteraction()
        {
            Utils.ToggleInput(Utils.ActionMapOptions.InAnimation, _playerInput);
            _cancelledAlready = false;
        }

        public override void CancelInteraction()
        {
            if (_cancelledAlready)  throw new Exception("You cannot cancel multiple times in a row");
            _cancelledAlready = true;
            Utils.ToggleInput(Utils.ActionMapOptions.Game, _playerInput);
        }
    }
    
    public abstract class PressInteractable : Interactable
    {
        public override void Interact(bool contextPerformed)
        {
            if (!contextPerformed) return;
            Interact();
        }
        protected abstract void Interact();
    }
}