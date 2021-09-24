using Photon.Pun;
using UnityEngine;
using Player_UI;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote player instances
	/// </summary>
	public class PlayerSetup : MonoBehaviour
	{
		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;
		[SerializeField] private GameObject nameText;

        [SerializeField] private Text nameText;

        private void Start()
        {

            PhotonView view = GetComponent<PhotonView>();
            if (view.IsMine) return;

            GameObject.Find("Canvas").GetComponent<PlayerStatistics>().player = gameObject;
            nameText.text = PhotonNetwork.NickName; // Haven't really tested this out yet, so not sure if it will actually work

			
			foreach (Behaviour behaviour in componentsToDisableIfNotMine) behaviour.enabled = false;
		}
	}
}