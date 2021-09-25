using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lobby
{
	public class PlayerListItem : MonoBehaviourPunCallbacks
	{
		#region Variables

		[SerializeField] private TMP_Text text;
		private                  Player   _player;

		#endregion

		#region Methods

		public void Setup(Player player)
		{
			_player = player;
			text.text = player.NickName;
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			if (Equals(_player, otherPlayer))
			{
				Destroy(gameObject);
			}
		}

		public override void OnLeftRoom()
		{
			Destroy(gameObject);
		}

		#endregion
	}
}