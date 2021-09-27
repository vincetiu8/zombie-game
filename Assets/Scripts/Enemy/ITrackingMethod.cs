using UnityEngine;

namespace Enemy
{
	// Making this really flexible to support just about any type of Player tracking you can think of in the future
	internal interface ITrackingMethod
	{
		void MoveTowardsPlayer(Transform player);
	}
}
