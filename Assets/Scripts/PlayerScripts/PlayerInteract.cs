using System;
using System.Collections.Generic;
using Interact;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace PlayerScripts
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
		private static readonly string[] IgnoredActions = { "Interact" };

		[Header("Interactable Update Settings")] [SerializeField]
		private float closestInteractableUpdateInterval = 0.25f;

		[SerializeField] private Transform localInteractableParent;

		[Header("Interactable Image Settings")] [SerializeField]
		private Image interactableImage;

		[SerializeField] private InteractableSpritesDict interactableSprites;
		[SerializeField] private Gradient                interactableProgressGradient;

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
			if (_interacting)
			{
				UpdateInteractableIcon();
				return;
			}

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
			_interactList.Add(other.gameObject);
			UpdateClosestInteractable();
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Interactable")) return;
			_interactList.Remove(other.gameObject);
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
			GameObject closestObject = null;
			float closestDistance = Mathf.Infinity;
			foreach (GameObject obj in _interactList)
			{
				// Give priority to interactables the player is currently holding
				if (obj.transform.parent == localInteractableParent)
				{
					closestObject = obj;
					_interacting = true;
					break;
				}

				if (Vector2.Distance(obj.transform.position, transform.position) >
				    closestDistance)
					continue;
				closestDistance = Vector2.Distance(obj.transform.position, transform.position);
				closestObject = obj;
			}

			_weaponsHandler.ToggleFireEnabled(!_interacting);

			Interactable newClosestInteractable = closestObject.GetComponent<Interactable>();

			if (_closestInteractable != newClosestInteractable)
			{
				if (_closestInteractable == null) interactableImage.enabled = true;

				interactableImage.sprite = interactableSprites[newClosestInteractable.GetInteractableType()];
				_closestInteractable = newClosestInteractable;
			}

			UpdateInteractableIcon();
		}

		private void UpdateInteractableIcon()
		{
			float progress = _closestInteractable.GetProgress();
			interactableImage.color = interactableProgressGradient.Evaluate(progress);
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
			_closestInteractable.StartInteraction();
		}

		public void CancelInteraction()
		{
			if (!_interacting) return;

			_closestInteractable.CancelInteraction();
		}

		private void OnStartInteraction()
		{
			ToggleInteraction(true);
			_closestInteractable.startInteraction.RemoveListener(OnStartInteraction);
			_closestInteractable.finishInteraction.AddListener(OnFinishInteraction);
			_interacting = true;
		}

		private void OnFinishInteraction()
		{
			ToggleInteraction(false);
			// todo: remove this null check
			if (_closestInteractable != null)
			{
				_closestInteractable.finishInteraction.RemoveListener(OnFinishInteraction);
			}

			_interacting = false;
			UpdateClosestInteractable();
		}

		private void ToggleInteraction(bool isInteracting)
		{
			MiscUtils.ToggleActions(_playerInput, IgnoredActions, !isInteracting);

			// Disable / enable player weapons
			_weaponsHandler.ToggleFireEnabled(!isInteracting);
		}
	}
}