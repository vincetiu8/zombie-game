using Interact;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	[RequireComponent(typeof(WindowController))]
	public class InteractableWindow : Interactable
	{
		[SerializeField] private float barricadeFixTime;
		private                  float _cooldown;

		private WindowController _windowController;

		protected override void Start()
		{
			base.Start();
			_windowController = GetComponent<WindowController>();
		}

		private void Update()
		{
			if (_cooldown > 0) _cooldown -= Time.deltaTime;
		}

		public override void Interact()
		{
			// Don't allow window to be repaired if a zombie is currently attacking it
			if (_windowController.zombieAtWindow || _cooldown > 0) return;

			_windowController.ChangeHealth(1);
			_cooldown += barricadeFixTime;
		}
	}
}