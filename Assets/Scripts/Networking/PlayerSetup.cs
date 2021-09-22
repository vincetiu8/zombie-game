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

		
		private void Start()
		{
			PhotonView view = GetComponent<PhotonView>();
			if (view.IsMine) return;
			
			HealthBarsLayout.Singleton.AddHealthController(GetComponent<Health>());
			nameText.GetComponent<TextMesh>().text = PhotonNetwork.NickName;

			
			foreach (Behaviour behaviour in componentsToDisableIfNotMine) behaviour.enabled = false;
		}
	}
}