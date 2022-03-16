using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
///     Handles animated explosions
/// </summary>
// Note: This script is unnecessary for particle based explosions as it relies on an AnimationClip
public class Explosion : MonoBehaviourPun
{
	[SerializeField] private AnimationClip explosionAnimation;
	[SerializeField] private string        explosionAnimTrigger;

	[SerializeField] private float     outerRadius      = 2f;
	[SerializeField] private float     initialIntensity = 1f;
	[SerializeField] private float     explosionAnimLengthMultiplier;
	private                  Animator  _anim;
	private                  Coroutine _explosionLightingCoroutine;
	private                  Light2D   _light;

	private void Awake()
	{
		_anim = GetComponent<Animator>();
		_light = GetComponent<Light2D>();
		_light.intensity = 0;
		_anim.SetFloat("explosionLen", explosionAnimLengthMultiplier);
		_anim.SetTrigger(explosionAnimTrigger);
		photonView.RPC("RpcExplosionLighting", RpcTarget.All);
	}

	[PunRPC]
	private void RpcExplosionLighting()
	{
		if (_explosionLightingCoroutine != null) StopCoroutine(_explosionLightingCoroutine);
		_explosionLightingCoroutine = StartCoroutine(ExplosionLighting());
	}

	private IEnumerator ExplosionLighting()
	{
		_light.intensity = initialIntensity;
		_light.pointLightOuterRadius = outerRadius;
		while (_light.intensity > 0)
		{
			_light.intensity -= Time.deltaTime / (explosionAnimLengthMultiplier * (float)1.5);
			yield return null;
		}

		_explosionLightingCoroutine = null;
		Destroy(gameObject);
	}
}