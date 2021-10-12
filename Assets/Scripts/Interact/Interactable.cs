using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Interact
{
	/// <summary>
	///     Interactable is the base class for all interactable objects.
	///     It handles players entering and exiting the intractable trigger area.
	///		All interactable objects have start and cancel methods.
	///		When an interactable finishes interacting, it emits the finishInteraction event.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public abstract class Interactable : MonoBehaviourPun
	{
		[HideInInspector] public UnityEvent finishInteraction;

		// Added for debugging convenience
		protected virtual void Start()
		{
			Collider2D[] cols = GetComponents<Collider2D>();
			if (!cols.Any(col => col.isTrigger))
				Debug.LogError("No trigger colliders attached to interactable object, can't interact");
		}

		/// <summary>
		///     Callback when this interactable is the closest one to the player.
		///     Used to display icons/gui, should show something to indicate interactable exists.
		/// </summary>
		public abstract void OnClosestInteractable();


		/// <summary>
		///     Callback when this interactable is not the closest one to the player.
		///     Used to remove icons/gui from when this was the closest interactable.
		/// </summary>
		public abstract void OnNotClosestInteractable();

		/// <summary>
		///     Callback when the player interacts with an object.
		///     We don't pass in the player, but it can be assumed that the interacting player is the local player.
		///     Abstract as all interactables need to respond to a start message.
		/// </summary>
		public virtual void StartInteraction()
		{
			// By default, interactables automatically finish as soon as they start
			finishInteraction.Invoke();
		}

		/// <summary>
		///     Only used by HoldInteractable.
		/// </summary>
		public virtual void CancelInteraction()
		{
		}
	}
}