using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Interact
{
	public enum InteractableType
	{
		Pickup,
		Repair,
		Shop
	}

	/// <summary>
	///     Interactable is the base class for all interactable objects.
	///     It handles players entering and exiting the intractable trigger area.
	///		All interactable objects have start and cancel methods.
	///		When an interactable finishes interacting, it emits the finishInteraction event.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public abstract class Interactable : MonoBehaviourPun
	{
		[SerializeField] private InteractableType interactableType;

		/// <summary>
		///     Used to signal the start of an interaction.
		///     Only used for hold interactions to lock the player input.
		/// </summary>
		[HideInInspector] public UnityEvent startInteraction;

		/// <summary>
		///     Used to signal the end of an interaction.
		///     Only used for hold interactions to unlock the player input.
		/// </summary>
		[HideInInspector] public UnityEvent finishInteraction;

		// Added for debugging convenience
		protected virtual void Start()
		{
			Collider2D[] cols = GetComponents<Collider2D>();
			if (!cols.Any(col => col.isTrigger))
				Debug.LogError("No trigger colliders attached to interactable object, can't interact");
		}

		public InteractableType GetInteractableType()
		{
			return interactableType;
		}

		/// <summary>
		///     Gets the current progress of the interaction.
		/// </summary>
		/// <returns>The current progress of the interaction</returns>
		public virtual float GetProgress()
		{
			// By default, just returns no progress.
			return 0;
		}

		/// <summary>
		///     Callback when the player interacts with an object.
		///     We don't pass in the player, but it can be assumed that the interacting player is the local player.
		///     Abstract as all interactables need to respond to a start message.
		/// </summary>
		public abstract void StartInteraction();

		/// <summary>
		///     Callback when the player cancels an interaction.
		/// Only used for HoldInteractions.
		/// </summary>
		public virtual void CancelInteraction()
		{
		}
	}
}