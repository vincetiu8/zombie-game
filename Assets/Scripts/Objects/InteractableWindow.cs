using System.ComponentModel;
using Interact;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	public class InteractableWindow : HoldInteractable
	{
		[Description("How fast health is restored to the barricade per second ")] [SerializeField] [Range(10, 100)]
		private int barricadeFixRate;

		private float _carryHealth;

		private WindowController _windowController;
		private int              _zombiesAtWindow;

		protected override void Start()
		{
			base.Start();
			_windowController = GetComponentInChildren<WindowController>();
		}

		private void Update()
		{
			if (!LocallyInteracting || _zombiesAtWindow > 0) return;

			_carryHealth += barricadeFixRate * Time.deltaTime;
			int intHealth = (int)_carryHealth;
			_windowController.ChangeHealth(intHealth);
			_carryHealth -= intHealth;

			if (_windowController.IsWindowFixed()) FinishInteraction();
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

		public override void StartInteraction()
		{
			if (_windowController.IsWindowFixed())
			{
				FinishInteraction();
				return;
			}

			_carryHealth = 0;

			base.StartInteraction();
		}
	}
}