using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Interact
{
	/// <summary>
	///     PlayerInteract handles player triggering interactable objects.
	/// </summary>
	public class PlayerInteract : MonoBehaviour
	{
		/// <summary>
		///     List to keep track of how many interactable objects are in range.
		/// </summary>
		private readonly List<GameObject> _interactPriorityList = new List<GameObject>();
        private GameObject _closestObject;
        
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
			_closestObject = null;
			foreach (GameObject obj in _interactPriorityList)
			{
				if (Vector2.Distance(obj.transform.position, transform.position) > closestDistance) continue;
				closestDistance = Vector2.Distance(obj.transform.position, transform.position);
				_closestObject = obj;
			}

			if (_closestObject == null) return;
            _closestObject.GetComponent<Interactable>().Interact();
        }

        public void CancelHoldInteractionAction(InputAction.CallbackContext context)
        {
            if (!context.canceled) return;
            CancelHoldInteraction();
        }

        /// <summary>
        /// Any script can call this to cancel the current player interaction
        /// </summary>
        public void CancelHoldInteraction()
        {
            if (!_closestObject) return;
            _closestObject.GetComponent<Interactable>().CancelInteraction();
        }
	}
}