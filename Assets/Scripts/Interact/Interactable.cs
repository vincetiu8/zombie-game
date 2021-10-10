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

		/// <summary>
		///     Callback when this interactable is the closest one to the player.
		///     Used to display icons/gui, should show something to indicate interactable exists.
		/// </summary>
		public abstract void OnClosestInteractable();


		/// <summary>
		///     Callback when this interactable is not the closest one to the player.
		///     Used to remove icons/gui from when this was the closest interactable.
		/// </summary>
		public abstract void OnNotClosestInteractable();

		/// <summary>
		///     Callback when the player interacts with an object.
		///     We don't pass in the player, but it can be assumed that the interacting player is the local player.
		///     Abstract as all interactions will require script
		/// </summary>
        protected internal abstract void StartInteraction( );

        /// <summary>
        ///     Only used by HoldInteractable.
        ///     Is virtual because if it's abstract,
        ///         every script that inherits it will need to throw the method exception even if they don't use it
        /// </summary>
        public virtual void CancelInteraction() { }
    }
    
    public abstract class HoldInteractable : Interactable
    {
        protected bool _locallyInteracting;
        private MiscUtils.ActionMapOptions _currentActionMap;
        private bool _avaliableForInteract = true;

        /// <summary>
        /// Prevents the player from firing and moving
        /// </summary>
        protected internal override void StartInteraction()
        {
            if (!_avaliableForInteract) return;
            ToggleInteraction(true);
        }
        
        /// <summary>
        /// Allows the player to move normally again, exception used to make sure this can only get called once to avoid repeat problems
        /// </summary>
        public override void CancelInteraction()
        {
            ToggleInteraction(false);
        }

        private void ToggleInteraction(bool startInteraction)
        {
            MiscUtils.ActionMapOptions actionMap = 
                startInteraction ? MiscUtils.ActionMapOptions.InAnimation : MiscUtils.ActionMapOptions.Game;
            
            // If actionMap to change is already current action map, cancel call
            if (_currentActionMap == actionMap) return;
            
            _currentActionMap = actionMap;
            
            // Change player action map
            MiscUtils.ToggleInput(actionMap, GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>());
            
            // Disable / enable player weapons
            GameManager.instance.localPlayerInstance.GetComponent<WeaponsHandler>().ToggleFireEnabled(!startInteraction);
            _locallyInteracting = startInteraction;
            photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, !startInteraction);

        }
        
        [PunRPC]
        protected void RPCSetAvailableForInteract(bool availableForInteract)
        {
            _avaliableForInteract = availableForInteract;
        }
    }
}