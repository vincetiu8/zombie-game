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
		[SerializeField] private Transform barricadeGraphics;

		private int              _activeBarricade;
		private List<GameObject> _barricades;
		private int              _healthPerBarricade;

		protected override void Awake()
		{
			base.Awake();

			_barricades = new List<GameObject>();
			foreach (Transform barricade in barricadeGraphics)
			{
				_barricades.Add(barricade.gameObject);
				barricade.gameObject.SetActive(true);
			}

			_activeBarricade = _barricades.Count - 1;
			_healthPerBarricade = Mathf.CeilToInt((float)initialHealth / _barricades.Count);
		}

		public override void ChangeHealth(int change)
		{
			int previousHealth = Health;
			int newHealth = Mathf.Clamp(Health + change, 0, initialHealth);
			if (previousHealth == newHealth) return;

			photonView.RPC("RPCChangeHealth", RpcTarget.All, newHealth);
		}

		[PunRPC]
		private void RPCChangeHealth(int newHealth)
		{
			int newActiveBarricade = (newHealth + _healthPerBarricade - 1) / _healthPerBarricade - 1;

			int min = Math.Min(_activeBarricade, newActiveBarricade);
			int max = Math.Max(_activeBarricade, newActiveBarricade);

			for (int i = min + 1; i <= max; i++) _barricades[i].SetActive(newActiveBarricade > _activeBarricade);

			_activeBarricade = newActiveBarricade;
			Health = newHealth;
		}
	}
}