using Interact;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	public class InteractableWindow : Interactable
	{
		[SerializeField] private float barricadeFixTime;
		private                  float _cooldown;

		private WindowController _windowController;
		private int              _zombiesAtWindow;

		protected override void Start()
		{
			base.Start();
			_windowController = GetComponentInChildren<WindowController>();
		}

		private void Update()
		{
			if (_cooldown > 0) _cooldown -= Time.deltaTime;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;

			_zombiesAtWindow++;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;

			_zombiesAtWindow--;
		}

		public override void Interact()
		{
			// Don't allow window to be repaired if a zombie is currently attacking it
			if (_zombiesAtWindow > 0 || _cooldown > 0) return;

			_windowController.ChangeHealth(1);
			_cooldown += barricadeFixTime;
		}
	}
}