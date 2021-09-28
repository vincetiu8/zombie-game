using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote player instances
	/// </summary>
	public class PlayerSetup : MonoBehaviourPun
	{
		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;

		private void Start()
		{
			if (photonView.IsMine)
			{
				PlayerNumbering.OnPlayerNumberingChanged += SetupPlayer;
				return;
			}

			foreach (Behaviour behaviour in componentsToDisableIfNotMine) behaviour.enabled = false;
		}

		private void SetupPlayer()
		{
			photonView.RPC("RPCSetupPlayer", RpcTarget.All);
		}

		[PunRPC]
		private void RPCSetupPlayer(PhotonMessageInfo info)
		{
			if (info.Sender.GetPlayerNumber() < 0) return;
			GameManager.instance.SetPlayerInstance(info.Sender.GetPlayerNumber(), gameObject);
		}
	}
}