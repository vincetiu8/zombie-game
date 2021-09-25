using Photon.Pun;
using UnityEngine;

namespace Shop
{
	public class PlayerGold : MonoBehaviour
	{
		#region Variables

		[HideInInspector] public GoldSystem goldSystem;

		#endregion


		#region Methods

		private void Start()
		{
			goldSystem.InitializePlayer(PhotonNetwork.NickName);
		}

		#endregion
	}
}