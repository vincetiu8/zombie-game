using System;
using System.Collections.Generic;
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
		[HideInInspector] public int zombiesAtWindow;

		[Header("Window Settings")] [Description("The graphics for each barricade")] [SerializeField]
		private Transform barricadeGraphics;

		[Description("The rate at which health is decreased")] [SerializeField] [Range(0, 1000)]
		private int barricadeBreakRate;

		private int _activeBarricade;

		private List<GameObject> _barricades;
		private float            _carryHealth;

		private void Start()
		{
			_barricades = new List<GameObject>();
			foreach (Transform barricade in barricadeGraphics)
			{
				_barricades.Add(barricade.gameObject);
				barricade.gameObject.SetActive(true);
			}

			_activeBarricade = _barricades.Count - 1;
		}


		private void OnCollisionStay2D(Collision2D collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
			Debug.Log("Colliding with enemy!");

			_carryHealth -= Time.deltaTime * barricadeBreakRate;
			int healthChange = (int)_carryHealth;
			ChangeHealth(healthChange);
			_carryHealth -= healthChange;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

			zombiesAtWindow++;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

			zombiesAtWindow--;
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
			int newActiveBarricade = Math.Min((newHealth + 1) / _barricades.Count, _barricades.Count - 1);

			int min = Math.Min(_activeBarricade, newActiveBarricade);
			int max = Math.Max(_activeBarricade, newActiveBarricade);
			for (int i = min; i < max; i++) _barricades[i].SetActive(newActiveBarricade > _activeBarricade);

			Health = newHealth;
		}
	}
}