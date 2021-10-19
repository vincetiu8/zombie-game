using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote player instances
	/// </summary>
	public class PlayerSetup : MonoBehaviourPun, IPunInstantiateMagicCallback
	{
		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;
		[SerializeField] private Text        nameText;


		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			GameManager.Instance.SetPlayerInstance(info.Sender.GetPlayerNumber(), gameObject);

			if (photonView.IsMine) return;

			// Will be false by default
			nameText.gameObject.SetActive(true);

			// Sets name tag to Nickname assigned at the launcher scene
			nameText.text = PhotonNetwork.NickName;

			foreach (Behaviour behaviour in componentsToDisableIfNotMine) behaviour.enabled = false;
		}
	}
}