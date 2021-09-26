using Interact;
using UnityEngine;

namespace Collectibles
{
	/// <summary>
	///     Represents an object that is destroyed when the player interacts (collects) it.
	/// </summary>
	public abstract class Collectible : Interactable
	{
		public override void Interact(GameObject player)
		{
			Pickup(player);
			Destroy(gameObject);
		}

		protected abstract void Pickup(GameObject player);
	}
}