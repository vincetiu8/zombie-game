using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interact
{
	/// <summary>
	///     PlayerInteract handles player triggering interactable objects.
	/// </summary>
	public class PlayerInteract : MonoBehaviour
	{
		[SerializeField] private float closestInteractableUpdateInterval = 0.25f;

		/// <summary>
		///     List to keep track of how many interactable objects are in range.
		/// </summary>
		private readonly List<GameObject> _interactList = new List<GameObject>();

		private Interactable _closestInteractable;
		private float        _cooldown;

		private void Update()
		{
			if (_cooldown > 0)
			{
				_cooldown -= Time.deltaTime;
				return;
			}

			UpdateClosestInteractable();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.CompareTag("Interactable")) return;
			AddInteractable(other.gameObject);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Interactable")) return;
			RemoveInteractable(other.gameObject);
		}

		// Added when we manually need to add objects
		// Hopefully we can remove this in the future, same as method below
		public void AddInteractable(GameObject interactable)
		{
			_interactList.Add(interactable);
			UpdateClosestInteractable();
		}

		public void RemoveInteractable(GameObject interactable)
		{
			_interactList.Remove(interactable);
			UpdateClosestInteractable();
		}

		private void UpdateClosestInteractable()
		{
			_cooldown = closestInteractableUpdateInterval;

			if (_interactList.Count == 0) return;

			// Check which object in the list is closest to the player
			float closestDistance = Mathf.Infinity;
			GameObject closestObject = null;
			foreach (GameObject obj in _interactList)
			{
				if (Vector2.Distance(obj.transform.position, transform.position) > closestDistance) continue;
				closestDistance = Vector2.Distance(obj.transform.position, transform.position);
				closestObject = obj;
			}

			if (closestObject == null)
			{
				_closestInteractable = null;
				return;
			}

			Interactable newClosestInteractable = closestObject.GetComponent<Interactable>();

			if (_closestInteractable == newClosestInteractable) return;

			if (_closestInteractable != null) _closestInteractable.OnNotClosestInteractable();

			_closestInteractable = newClosestInteractable;
			_closestInteractable.OnClosestInteractable();
		}


		public void CheckInteraction(InputAction.CallbackContext context)
		{
			if (!context.performed || _closestInteractable == null) return;

			_closestInteractable.Interact();
		}
	}
}