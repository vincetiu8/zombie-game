using System.ComponentModel;
using Interact;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	public class InteractableWindow : IconInteractable
	{
		[Description("How fast health is restored to the barricade")] [SerializeField] [Range(1, 100)]
		private int barricadeFixRate;

		private WindowController _windowController;
		private int              _zombiesAtWindow;

		protected override void Start()
		{
			base.Start();
			_windowController = GetComponentInChildren<WindowController>();
		}

		protected void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;

			_zombiesAtWindow++;
		}

		protected void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;

			_zombiesAtWindow--;
		}

		public override void Interact()
		{
			// Don't allow window to be repaired if a zombie is currently attacking it
			if (_zombiesAtWindow > 0) return;

			// todo: fix this later to use hold interact
			_windowController.ChangeHealth(barricadeFixRate);
		}
	}
}