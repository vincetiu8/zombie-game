using Photon.Pun;
using UnityEngine;

namespace Shop
{
	public class PlayerGold : MonoBehaviour
	{
		[HideInInspector] public GoldSystem goldSystem;

		private void Start()
		{
			goldSystem.InitializePlayer(PhotonNetwork.NickName);
		}
	}
}