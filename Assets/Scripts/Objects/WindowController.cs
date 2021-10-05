using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     Handles zombies breaking the window.
	/// </summary>
	public class WindowController : HealthController
	{
		[HideInInspector] public bool zombieAtWindow;

		[Header("Window Settings")] [Description("The graphics for each barricade")] [SerializeField]
		private GameObject barricadesGraphics;

		[Description("The window's collider")] [SerializeField]
		private GameObject windowCollider;

		[Description("The rate at which health is decreased")] [SerializeField] [Range(0, 1000)]
		private int barricadeBreakRate;

		private float _carryHealth;

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

			zombieAtWindow = false;
		}


		private void OnTriggerStay2D(Collider2D collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
			_carryHealth -= Time.deltaTime * barricadeBreakRate;
			int healthChange = (int)_carryHealth;
			ChangeHealth(healthChange);
			_carryHealth -= healthChange;
			zombieAtWindow = true;
		}

		public override void ChangeHealth(int change)
		{
			int previousHealth = Health;
			int newHealth = Mathf.Clamp(Health + change, 0, initialHealth);
			if (previousHealth == newHealth) return;

			photonView.RPC("RPCChangeHealth", RpcTarget.All, newHealth);
		}

		private void RPCChangeHealth(int newHealth)
		{
			windowCollider.SetActive(newHealth != 0);

			if (newHealth - Health < 0 && Mathf.CeilToInt(newHealth) != 6)
				barricadesGraphics.transform.GetChild(Mathf.CeilToInt(newHealth)).gameObject.SetActive(false);
			else if (newHealth - Health > 0)
				barricadesGraphics.transform.GetChild(Health - 1).gameObject.SetActive(true);

			Health = newHealth;
		}
	}
}