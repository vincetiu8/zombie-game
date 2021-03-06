using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     Handles zombies breaking the window.
	/// </summary>
	public class WindowController : HealthController
	{
		[Header("Barricade Settings")] [SerializeField]
		private Transform barricadeGraphics;

		[SerializeField] [Range(0, 500)] private int maxHealth;

		private int              _activeBarricade;
		private List<GameObject> _barricades;
		private int              _healthPerBarricade;

		private void Start()
		{
			_barricades = new List<GameObject>();
			foreach (Transform barricade in barricadeGraphics)
			{
				_barricades.Add(barricade.gameObject);
				barricade.gameObject.SetActive(true);
			}

			_activeBarricade = _barricades.Count - 1;
			_healthPerBarricade = Mathf.CeilToInt((float)maxHealth / _barricades.Count);

			// Update the barricade graphics
			Health = maxHealth;
			RPCChangeHealth(initialHealth, 0);
		}

		public override void ChangeHealth(int change)
		{
			int newHealth = Mathf.Clamp(Health + change, 0, maxHealth);
			if (Health == newHealth) return;
			photonView.RPC("RPCChangeHealth", RpcTarget.All, newHealth, change);
		}

		public int GetMaxHealth()
		{
			return maxHealth;
		}

		[PunRPC]
		protected override void RPCChangeHealth(int newHealth, int change)
		{
			int newActiveBarricade = (newHealth + _healthPerBarricade - 1) / _healthPerBarricade - 1;

			int min = Math.Min(_activeBarricade, newActiveBarricade);
			int max = Math.Max(_activeBarricade, newActiveBarricade);

			for (int i = min + 1; i <= max; i++) _barricades[i].SetActive(newActiveBarricade > _activeBarricade);

			_activeBarricade = newActiveBarricade;
			if (change != 0) SpawnHealthPopup(newHealth - Health);
			Health = newHealth;
		}

		public bool IsWindowFixed()
		{
			return Health == maxHealth;
		}
	}
}