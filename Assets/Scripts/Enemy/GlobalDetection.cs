using UnityEngine;

namespace Enemy
{
	public class GlobalDetection : PlayerDetector
	{
		/// <summary>
		///  Every player is permanently in the player detection list
		/// </summary>
		private GameObject[] _players;
		private void Start()
		{
			_players = GameObject.FindGameObjectsWithTag("Player");

			foreach (GameObject player in _players)
			{
				AddPlayer(player.transform);
			}
		}
	}
}
