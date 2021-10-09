using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote player instances
	/// </summary>
	public class PlayerSetup : MonoBehaviourPun, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy
	{
		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;
		[SerializeField] private Text        nameText;

		public void OnPreNetDestroy(PhotonView rootView)
		{
			Debug.Log("dying...");
			GameManager.Instance.RemovePlayerInstance(rootView.Owner.GetPlayerNumber());
			GameManager.Instance.GetComponent<SpectatorManager>().enabled = true;
		}

		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			photonView.AddCallbackTarget(this);
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