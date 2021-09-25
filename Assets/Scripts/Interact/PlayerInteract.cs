using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interact
{
	// PlayerInteract handles player triggering interactable objects
	public class PlayerInteract : MonoBehaviour
	{
		#region Variables

		// List to keep track of how many interactable objects are in rang
		private readonly List<GameObject> _interactPriorityList = new List<GameObject>();

		#endregion

		#region Methods

		public void AddInteractableObject(GameObject interact)
		{
			_interactPriorityList.Add(interact);
		}

		public void RemoveInteractableObject(GameObject interact)
		{
			_interactPriorityList.Remove(interact);
		}

		public void CheckInteraction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;
			if (_interactPriorityList.Count <= 0) return;

			// Check which object in the list is closest to the player
			float closestDistance = Mathf.Infinity;
			GameObject closestObject = null;
			foreach (GameObject obj in _interactPriorityList)
			{
				if (Vector2.Distance(obj.transform.position, transform.position) > closestDistance) continue;
				closestDistance = Vector2.Distance(obj.transform.position, transform.position);
				closestObject = obj;
			}

			if (closestObject == null) return;

			closestObject.GetComponent<Interactable>().Interact(gameObject);
		}

		#endregion
	}
}