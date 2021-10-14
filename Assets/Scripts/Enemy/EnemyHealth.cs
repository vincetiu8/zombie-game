using Networking;
using Objects;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Enemy
{
	public class EnemyHealth : HealthController
	{
		[Header("Enemy Settings")] [SerializeField] [Range(0, 1000)]
		private int goldDropAmount;

		protected override void OnDeath()
		{
			// It is safe to assume the local player killed the enemy
			// Remote players would call this method locally
			GameManager.Instance.goldSystem.AddGold(goldDropAmount, PhotonNetwork.LocalPlayer.GetPlayerNumber());

			base.OnDeath();
		}
	}
}