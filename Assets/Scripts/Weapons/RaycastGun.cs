using System.Collections;
using System.ComponentModel;
using Objects;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;

namespace Weapons
{
	public class RaycastGun : Gun
	{
		private const float PositionPrecision = 100;

		[Header("Raycast Settings")]
		[Description("The distance to raycast over when looking for enemies")]
		[SerializeField]
		private float shotDistance;

		[SerializeField] private GameObject tracerPrefab;

		private LayerMask _layerMask;
		private Coroutine _muzzleFlashCoroutine;

		private void Awake()
		{
			_layerMask = LayerMask.GetMask("Enemies", "Objects", "Obstacles");
		}

		protected override void FireBullet(float angle)
		{
			Vector2 direction = TransformUtils.DegToVector2(angle);
			RaycastHit2D hit = Physics2D.Raycast(firepoint.position, direction, shotDistance, _layerMask);

			Vector2 endpoint = hit.point;

			if (hit.collider == null) endpoint = (Vector2)firepoint.position + direction * shotDistance;

			endpoint *= PositionPrecision;
			photonView.RPC("RPCFireBullet", RpcTarget.All, (int)endpoint.x, (int)endpoint.y);

			if (hit.collider == null || !MiscUtils.IsInLayerMask(_layerMask, hit.collider.gameObject.layer)) return;

			HealthController healthController = hit.collider.gameObject.GetComponent<HealthController>();

			if (healthController == null) return;

			healthController.ChangeHealth(-currentAttributes.damage);

            if (healthController.transform.GetComponent<KnockbackController>() == null) return;
            
            healthController.transform.GetComponent<KnockbackController>().TakeKnockBack(angle, currentAttributes.knockback);
		}

		[PunRPC]
		protected void RPCFireBullet(int x, int y)
		{
			GameObject tracerInstance = Instantiate(tracerPrefab, Vector3.zero, Quaternion.identity);
			LineRenderer lineRenderer = tracerInstance.GetComponent<LineRenderer>();
			lineRenderer.SetPosition(0, firepoint.position);

			Vector2 endpoint = new Vector2(x, y) / PositionPrecision;
			lineRenderer.SetPosition(1, endpoint);
			
			if(_muzzleFlashCoroutine != null) StopCoroutine(_muzzleFlashCoroutine);
			_muzzleFlashCoroutine = StartCoroutine(MuzzleFlash());
		}

		private IEnumerator MuzzleFlash()
		{
			Light2D _light = GetComponentInChildren<Light2D>();
			_light.intensity = 2;
			while (_light.intensity > 0)
			{
				_light.intensity -= Time.deltaTime * 8;
				yield return null;
			}

			_muzzleFlashCoroutine = null;
		}
	}
}