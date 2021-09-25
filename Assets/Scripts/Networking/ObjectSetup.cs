using Photon.Pun;
using UnityEngine;

namespace Networking
{
	// ObjectSetup disables behaviour on remote players
	public class ObjectSetup : MonoBehaviour
	{
		#region Variables

		[Header("Setup Configuration")] [SerializeField]
		private Behaviour[] componentsToDisableIfNotMasterClient;

		#endregion

		#region Methods

		private void Start()
		{
			if (PhotonNetwork.IsMasterClient) return;

			foreach (Behaviour behaviour in componentsToDisableIfNotMasterClient) behaviour.enabled = false;
		}

		#endregion
	}
}