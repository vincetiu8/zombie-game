using UnityEngine;

namespace Weapons
{
	public class Shotgun : RaycastGun
	{
		[SerializeField] private int   pelletAmount;
		[SerializeField] private float spraySpread;

		protected override void FireBullets()
		{
			float direction = firepoint.rotation.eulerAngles.z;
			for (int i = 0; i < pelletAmount; i++)
			{
				float firingAngle = direction + Random.Range(-spraySpread, spraySpread);
				FireBullet(firingAngle);
			}
		}
	}
}