using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Weapons;

namespace Interact
{
	[Serializable]
	public class InteractableSpritesDict : SerializableDictionary<InteractableType, Sprite>
	{
	}

	/// <summary>
	///     PlayerInteract handles player triggering interactable objects.
	/// </summary>
	public class PlayerInteract : MonoBehaviour
	{
		[Header("Interactable Update Settings")] [SerializeField]
		private float closestInteractableUpdateInterval = 0.25f;

		[Header("Interactable Image Settings")] [SerializeField]
		private Image interactableImage;

		[SerializeField] private InteractableSpritesDict interactableSprites;

		/// <summary>
		///     List to keep track of how many interactable objects are in range.
		/// </summary>
		private readonly List<GameObject> _interactList = new List<GameObject>();

		private Interactable _closestInteractable;
		private float        _cooldown;
		private bool         _interacting;

		private PlayerInput    _playerInput;
		private WeaponsHandler _weaponsHandler;

		private void Start()
		{
			_weaponsHandler = GetComponent<WeaponsHandler>();
			_playerInput = GetComponent<PlayerInput>();
		}

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
			if (_interacting) return;

			_cooldown = closestInteractableUpdateInterval;

			if (_interactList.Count == 0)
			{
				if (_closestInteractable != null) interactableImage.enabled = false;

				_closestInteractable = null;
				return;
			}

			// Check which object in the list is closest to the player
			GameObject closestObject = _interactList[0];
			float closestDistance = Vector2.Distance(closestObject.transform.position, transform.position);
			for (int index = 1; index < _interactList.Count; index++)
			{
				GameObject obj = _interactList[index];
				if (Vector2.Distance(obj.transform.position, transform.position) > closestDistance) continue;
				closestDistance = Vector2.Distance(obj.transform.position, transform.position);
				closestObject = obj;
			}

			Interactable newClosestInteractable = closestObject.GetComponent<Interactable>();

			if (_closestInteractable == newClosestInteractable) return;

			interactableImage.enabled = true;
			interactableImage.sprite = interactableSprites[newClosestInteractable.GetInteractableType()];

			_closestInteractable = newClosestInteractable;
		}


		public void InteractionAction(InputAction.CallbackContext context)
		{
			if (_closestInteractable == null || context.started) return;

			if (context.canceled)
			{
				CancelInteraction();
				return;
			}

			_closestInteractable.startInteraction.AddListener(OnStartInteraction);
			_closestInteractable.finishInteraction.AddListener(OnFinishInteraction);
			_closestInteractable.StartInteraction();
		}

		public void CancelInteraction()
		{
			if (!_interacting) return;

			_closestInteractable.CancelInteraction();
			Debug.Log("Cancelling!");
		}

		private void OnStartInteraction()
		{
			Debug.Log("Starting Interaction!");
			_interacting = true;
			ToggleInteraction(true);
		}

		private void OnFinishInteraction()
		{
			_interacting = false;
			ToggleInteraction(false);
			_closestInteractable.startInteraction.RemoveListener(OnStartInteraction);
			_closestInteractable.finishInteraction.RemoveListener(OnFinishInteraction);
		}

		private void ToggleInteraction(bool isInteracting)
		{
			foreach (InputAction action in _playerInput.currentActionMap.actions)
			{
				if (action.name == "Interact") continue;

				if (isInteracting)
				{
					action.Disable();
					continue;
				}

				action.Enable();
			}

			// Disable / enable player weapons
			_weaponsHandler.ToggleFireEnabled(!isInteracting);
		}
	}
}