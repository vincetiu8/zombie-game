using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Weapons
{
	public class ProjectileLauncher : Gun
	{
		[Header("Projectile Settings")] [Description("The bullet prefab to be instantiated")] [SerializeField]
		private GameObject bulletPrefab;

		[Description("The amount of force to apply to the projectile")] [SerializeField]
		private float throwForce;

		protected override void FireBullet(float angle)
		{
			Quaternion rotation = Quaternion.Euler(0, 0, angle);
			GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, rotation);

			// Set the bullet's attributes
			Vector2 direction = TransformUtils.DegToVector2(angle);
			bulletClone.GetComponent<Rigidbody2D>().AddForce(direction * throwForce, ForceMode2D.Impulse);
			bulletClone.GetComponent<BulletController>().damage = currentAttributes.damage;
		}
	}
}