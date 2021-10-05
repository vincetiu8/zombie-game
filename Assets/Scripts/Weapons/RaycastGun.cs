using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Weapons
{
	public class RaycastGun : Gun
	{
		[Header("Raycast Settings")]
		[Description("The distance to raycast over when looking for enemies")]
		[SerializeField]
		private float shotDistance;

		[SerializeField] private GameObject tracerPrefab;

		private LayerMask _layerMask;

		private void Awake()
		{
			_layerMask = LayerMask.GetMask("Enemy", "Objects", "Obstacles", "Player");
		}

		protected override void FireBullet(float angle)
		{
			Vector2 direction = TransformUtils.DegToVector2(angle);
			RaycastHit2D hit = Physics2D.Raycast(firepoint.position, direction, shotDistance, _layerMask);

			Vector2 endpoint = hit.point;

			if (hit.collider == null) endpoint = (Vector2)firepoint.position + direction * shotDistance;

			photonView.RPC("RPCFireBullet", RpcTarget.All, endpoint);

			if (hit.collider == null || !hit.collider.CompareTag("Enemy")) return;

			HealthController healthController = hit.collider.gameObject.GetComponent<HealthController>();

			if (healthController == null) return;

			healthController.ChangeHealth(-currentAttributes.damage);
		}

		[PunRPC]
		protected void RPCFireBullet(Vector2 endpoint)
		{
			GameObject tracerInstance = Instantiate(tracerPrefab, Vector3.zero, Quaternion.identity);
			LineRenderer lineRenderer = tracerInstance.GetComponent<LineRenderer>();
			lineRenderer.SetPosition(0, firepoint.position);
			lineRenderer.SetPosition(1, endpoint);
		}
	}
}