using Photon.Pun;
using UnityEngine;

namespace Networking
{
	/// <summary>
	///     Disables behaviours on remote players
	/// </summary>
	public class ObjectSetup : MonoBehaviour
	{
		[Header("Setup Configuration")] [SerializeField]
		private Behaviour[] componentsToDisableIfNotMasterClient;

		private void Start()
		{
			if (PhotonNetwork.IsMasterClient) return;

			foreach (Behaviour behaviour in componentsToDisableIfNotMasterClient) behaviour.enabled = false;
		}
	}
}