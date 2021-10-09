using Photon.Realtime;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	///     Every player is permanently in the player detection list.
	/// </summary>
	public class GlobalDetection : PlayerDetector
	{
		private void Start()
		{
			ClearPlayers();

			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

			foreach (GameObject player in players)
			{
				AddPlayer(player.transform);
			}
		}

		public void OnPlayerEnteredRoom(Player newPlayer)
		{
			Start();
		}

		public void OnPlayerLeftRoom(Player otherPlayer)
		{
			Start();
		}
	}
}