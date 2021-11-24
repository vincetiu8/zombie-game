using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Weapons
{
	public class ProjectileLauncher : Gun
	{
		[Header("Projectile Settings")] [Description("The bullet prefab to be instantiated")] [SerializeField]
		protected GameObject bulletPrefab;

		[Description("The amount of force to apply to the projectile")] [SerializeField]
		protected float throwForce;

		protected override void FireBullet(float angle, bool alt)
		{
			Quaternion rotation = Quaternion.Euler(0, 0, angle);
			GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, rotation);
			if (alt)
			{
				AltFireAction(bulletClone, angle);
				return;
			}
			DefaultFireAction(bulletClone, angle);
			
		}

		protected virtual void DefaultFireAction(GameObject bulletClone, float angle)
		{
			SetBulletAttributes(bulletClone, angle, 1);
		}

		protected virtual void AltFireAction(GameObject bulletClone, float angle)
		{
			
		}

		protected virtual void SetBulletAttributes(GameObject bulletClone, float angle, float forceMultiplier)
		{
			// Set the bullet's attributes
			Vector2 direction = TransformUtils.DegToVector2(angle);
			bulletClone.GetComponent<Rigidbody2D>().AddForce(direction * (throwForce * forceMultiplier), ForceMode2D.Impulse);
			bulletClone.GetComponent<BulletController>().damage = currentAttributes.damage;
		}
	}
}