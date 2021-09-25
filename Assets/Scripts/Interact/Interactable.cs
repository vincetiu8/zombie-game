using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Interact
{
	// Interactable is the base class for all interactable objects
	// It handles players entering and exiting the intractable trigger area
	[RequireComponent(typeof(Collider2D))]
	public abstract class Interactable : MonoBehaviourPun
	{
		#region Methods

		// Added for debugging convenience
		private void Start()
		{
			Collider2D[] cols = GetComponents<Collider2D>();
			if (cols.Any(col => col.isTrigger)) return;

			Debug.LogError("No trigger colliders attached to interactable object, can't interact.");
		}

		public abstract void Interact(GameObject player);

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
				collision.GetComponent<PlayerInteract>()
				         .AddInteractableObject(gameObject); // Display the icon to let player know the object is interactable
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
				collision.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
		}

		#endregion
	}
}