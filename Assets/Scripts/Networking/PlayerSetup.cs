using Photon.Pun;
using UnityEngine;
using Player_UI;
using UnityEngine.UI;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote player instances
	/// </summary>
	public class PlayerSetup : MonoBehaviour
	{
		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;
        [SerializeField] private Text nameText;

        private void Start()
        {

            PhotonView view = GetComponent<PhotonView>();
            if (view.IsMine) return;

            // Will be false by default
            nameText.gameObject.SetActive(true);
            
            // Sets name tag to Nickname assigned at the launcher scene
            nameText.text = PhotonNetwork.NickName;
			
			foreach (Behaviour behaviour in componentsToDisableIfNotMine) behaviour.enabled = false;
		}
	}
}