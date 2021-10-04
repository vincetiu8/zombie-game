using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	///     Makes the enemy move directly towards the player it's targeting.
	/// </summary>

	public abstract class ChaserAI : MonoBehaviour
	{
        protected Transform _playerTracked;
        public virtual void SetPlayerToTrack(Transform player)
        {
            _playerTracked = player;
        }

	}
}
