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
		private PlayerInteract _interact;

		// Added for debugging convenience
		protected virtual void Start()
		{
			Collider2D[] cols = GetComponents<Collider2D>();
			if (!cols.Any(col => col.isTrigger))
				Debug.LogError("No trigger colliders attached to interactable object, can't interact");
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))

			_interact = collision.GetComponent<PlayerInteract>();
			
			// Check if collision is a player
			if(_interact == null) return;
			_interact.AddInteractableObject(gameObject);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))

			_interact = collision.GetComponent<PlayerInteract>();
 
			if(_interact == null) return;
			_interact.RemoveInteractableObject(gameObject);
		}

		/// <summary>
		///     Callback when the player interacts with an object.
		///     We don't pass in the player, but it can be assumed that the interacting player is the local player.
		/// </summary>
		public abstract void Interact();
	}
}