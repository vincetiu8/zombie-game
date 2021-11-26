using System.ComponentModel;
using Interact;
using Photon.Pun;
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
			_windowController = GetComponentInChildren<WindowController>();
			base.Start();
		}

		private void Update()
		{
			if (!LocallyInteracting || _zombiesAtWindow > 0) return;

			_carryHealth += barricadeFixRate * Time.deltaTime;
			int intHealth = (int)_carryHealth;
			_windowController.ChangeHealth(intHealth);
			_carryHealth -= intHealth;

			if (!_windowController.IsWindowFixed()) return;

			FinishInteraction();
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

		public override float GetProgress()
		{
			return (float)_windowController.GetHealth() / _windowController.GetMaxHealth();
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

		[PunRPC]
		protected override void RPCSetAvailableForInteract(bool available)
		{
			AvailableForInteract = available && !_windowController.IsWindowFixed();
		}
	}
}