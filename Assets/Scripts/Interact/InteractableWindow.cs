using Objects;
using UnityEngine;

namespace Interact
{
	// InteractableWindow implements the repair mechanic on windows
	[RequireComponent(typeof(WindowController))]
	public class InteractableWindow : Interactable
	{
		#region Variables

		[SerializeField] private float barricadeFixTime;

		private WindowController _windowController;

		private float _cooldown;

		#endregion

		#region Methods

		public override void Interact(GameObject player)
		{
			// Don't allow window to be repaired if a zombie is currently attacking it
			if (_windowController.zombieAtWindow || _cooldown > 0) return;

			_windowController.ChangeHealth(1);
			_cooldown += barricadeFixTime;
		}

		private void Start()
		{
			_windowController = GetComponent<WindowController>();
		}

		private void Update()
		{
			if (_cooldown > 0) _cooldown -= Time.deltaTime;
		}

		#endregion
	}
}