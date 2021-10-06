namespace Enemy
{
	/// <summary>
	/// Makes the enemy move directly towards the player it's targeting.
	/// </summary>
	public class StraightTracking : ChaserAI
	{
		private void Update()
		{
			Destination = TrackingPlayer.position;
		}
	}
}