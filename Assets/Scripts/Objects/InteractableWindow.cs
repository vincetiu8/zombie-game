using System.ComponentModel;
using Interact;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Shop;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	public class InteractableWindow : HoldInteractable
	{
		[Description("How much health is restored to the barricade per update")] [SerializeField] [Range(10, 100)]
		private int barricadeFixAmount;

		[Description("How often health is restored to the barricade")] [SerializeField] [Range(0.5f, 5f)]
		private float barricadeFixInterval;

		private float            _fixDelay;
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

			_fixDelay -= Time.deltaTime;

			if (_fixDelay <= 0)
			{
				_windowController.ChangeHealth(barricadeFixAmount);
				_fixDelay = barricadeFixInterval;
			}

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
				GameManager.Instance.GetComponent<GoldSystem>().AddGold(10, PhotonNetwork.LocalPlayer.GetPlayerNumber());
				return;
			}

			_fixDelay = barricadeFixInterval;

			base.StartInteraction();
		}

		[PunRPC]
		protected override void RPCSetAvailableForInteract(bool available)
		{
			AvailableForInteract = available && !_windowController.IsWindowFixed();
		}
	}
}