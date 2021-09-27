using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote player instances
	/// </summary>
	public class PlayerSetup : MonoBehaviourPun
	{
		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;
        [SerializeField] private Text nameText;

		private void Start()
		{
			if (photonView.IsMine)
			{
				PlayerNumbering.OnPlayerNumberingChanged += SetupPlayer;
				return;
			}

            // Will be false by default
            nameText.gameObject.SetActive(true);
            
            // Sets name tag to Nickname assigned at the launcher scene
            nameText.text = PhotonNetwork.NickName;
			
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