using Photon.Pun;
using UnityEngine;

namespace Networking
{
	// PlayerSetup disables components on remote player instances
	public class PlayerSetup : MonoBehaviour
	{
		#region Variales

		[SerializeField] private Behaviour[] componentsToDisableIfNotMine;

		#endregion

		#region Methods

		private void Start()
		{
			PhotonView view = GetComponent<PhotonView>();
			if (view.IsMine) return;

			foreach (Behaviour behaviour in componentsToDisableIfNotMine) behaviour.enabled = false;
		}

		#endregion
	}
}