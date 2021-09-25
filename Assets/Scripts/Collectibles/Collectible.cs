using Interact;
using UnityEngine;

namespace Collectibles
{
	// Collectible represents an object that is destroyed when the player interacts (collects) it
	public abstract class Collectible : Interactable
	{
		#region Methods

		public override void Interact(GameObject player)
		{
			Pickup(player);
			Destroy(gameObject);
		}

		protected abstract void Pickup(GameObject player);

		#endregion
	}
}