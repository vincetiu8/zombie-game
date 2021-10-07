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
        private                  PlayerInput _playerInput;
        private WeaponsHandler _playerWeaponHandler;
        
        private bool _cancelledAlready = true;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(LateStart());
        }
        // Done to fix script execution order problems
        private IEnumerator LateStart()
        {
            yield return new WaitUntil(() => GameManager.instance.localPlayerInstance != null);
            Debug.Log("found the player");
            _playerInput = GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>();
            _playerWeaponHandler = GameManager.instance.localPlayerInstance.GetComponent<WeaponsHandler>();
        }

        protected internal override void Interact()
        {
          StartInteraction();
        }
        
        /// <summary>
        /// Prevents the player from firing and moving
        /// </summary>
        protected virtual void StartInteraction()
        {
            MiscUtils.ToggleInput(MiscUtils.ActionMapOptions.InAnimation, _playerInput);
            _playerWeaponHandler.ToggleFireEnabled(false);
            _cancelledAlready = false;
        }
        
        /// <summary>
        /// Allows the player to move normally again, execption used to make sure this can only get called once to avoid repeat problems
        /// </summary>
        public override void CancelInteraction()
        {
            if (_cancelledAlready)  throw new Exception("You cannot cancel multiple times in a row");
            _cancelledAlready = true;
            MiscUtils.ToggleInput(MiscUtils.ActionMapOptions.Game, _playerInput);
            _playerWeaponHandler.ToggleFireEnabled(true);
        }
    }

}