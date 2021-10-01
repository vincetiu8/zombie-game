using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

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
		public abstract void Interact(bool contextPerformed);
	}
    
    public abstract class HoldInteractable : Interactable
    {
        public override void Interact(bool contextPerformed)
        {
            if (contextPerformed)
            {
                StartInteraction();
                return;
            }
            CancelInteraction();
        }
        protected abstract void StartInteraction();
        public abstract void CancelInteraction();
    }

    // I'm considering restructuring every press interaction to what i wrote below,
    // However the code will get alot messier and longer because now you need to fill out 2 methods instead of just one
    // What do you say? (the StartInteraction and CancelInteraction will be moved to Interactable class if you say yes ofc)
    public abstract class PressInteractable : Interactable
    {
        public override void Interact(bool contextPerformed)
        {
            (contextPerformed ? (Action)StartInteraction : CancelInteraction)();
        }
        protected abstract void StartInteraction();

        public abstract void CancelInteraction();
    }
}