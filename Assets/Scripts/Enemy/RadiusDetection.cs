using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


namespace Enemy
{
	/// <summary>
	///  Only includes players that are within radius of the gameObject in the player detection list
	/// </summary>
	public class RadiusDetection : PlayerDetector
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.CompareTag("Player")) return;
			AddPlayer(other.transform);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Player")) return;

			RemovePlayer(other.transform);
		}

	}
}
