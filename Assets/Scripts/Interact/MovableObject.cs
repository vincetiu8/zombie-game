using UnityEngine;
using Weapons;

namespace Interact
{
	// Movable object represents an object the player can pickup and move
	public class MovableObject : Interactable
	{
		#region Variables

		private bool         _isHolding;
		private Collider2D[] _colList;

		#endregion

		#region Methods

		private void Awake()
		{
			_colList = transform.GetComponentsInChildren<Collider2D>();
		}

		public override void Interact(GameObject player)
		{
			_isHolding = !_isHolding;
			SetAllCollidersStatus(!_isHolding);

			// Prevent the player from using any weapons
			player.GetComponent<WeaponsHandler>().PreventFire(_isHolding);
			transform.SetParent(_isHolding ? player.transform.Find("PlayerObject").gameObject.transform : null);

			// When the colliders are disabled, it removes this from the interactable list
			// We need to add it back so the player can drop the item and vice versa
			// This also means closer objects will be interacted with instead of dropping this objects
			// Therefore, it is suggested to make the interactable trigger as small as possible
			if (_isHolding)
			{
				player.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
				return;
			}

			player.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
		}

		private void SetAllCollidersStatus(bool active)
		{
			foreach (Collider2D colliders in _colList) colliders.enabled = active;
		}

		#endregion
	}
}