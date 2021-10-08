using System;
using System.Collections;
using System.Linq;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Weapons;

namespace Interact
{
	/// <summary>
	///     Interactable is the base class for all interactable objects.
	///     It handles players entering and exiting the intractable trigger area.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public abstract class Interactable : MonoBehaviourPun
    {
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
        protected internal abstract void Interact( );

        public virtual void CancelInteraction() { }
    }
    
    public abstract class HoldInteractable : Interactable
    {
        protected bool _currentlyInteracting;
        
        protected internal override void Interact()
        {
          StartInteraction();
        }
        
        /// <summary>
        /// Prevents the player from firing and moving
        /// </summary>
        protected virtual void StartInteraction()
        {
            MiscUtils.ToggleInput(MiscUtils.ActionMapOptions.InAnimation, GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>());
            GameManager.instance.localPlayerInstance.GetComponent<WeaponsHandler>().ToggleFireEnabled(false);
            _currentlyInteracting = true;
        }
        
        /// <summary>
        /// Allows the player to move normally again, execption used to make sure this can only get called once to avoid repeat problems
        /// </summary>
        public override void CancelInteraction()
        {
            MiscUtils.ToggleInput(MiscUtils.ActionMapOptions.Game, GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>());
            GameManager.instance.localPlayerInstance.GetComponent<WeaponsHandler>().ToggleFireEnabled(true);
            _currentlyInteracting = false;
        }
    }

}