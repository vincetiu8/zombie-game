using System.Collections;
using System.ComponentModel;
using Interact;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
	/// <summary>
	///     Handles changing and syncing the lighting of the lanterns.
	/// </summary>
	public class Lantern : TimedInteractable
	{
		private const float DurationOnAllDead = 2;

		[Header("Lantern Settings")] [Description("The duration the lantern stays on")] [SerializeField] [Range(1, 30)]
		private float duration;

		private Coroutine _fadeLightingCoroutine;
		private Light2D   _light2D;

		protected override void Start()
		{
			base.Start();
			_light2D = GetComponent<Light2D>();
			_light2D.intensity = 0;
			GameManager.Instance.onAllPlayersDead.AddListener(FadeOnDeath);
		}

		protected override void OnSuccessfulInteraction()
		{
			photonView.RPC("RpcStartLightingCoroutine", RpcTarget.All);
			FinishInteraction();
		}

		[PunRPC]
		private void RpcStartLightingCoroutine()
		{
			if (_fadeLightingCoroutine != null) StopCoroutine(_fadeLightingCoroutine);
			_fadeLightingCoroutine = StartCoroutine(FadeLightingCoroutine());
		}

		private IEnumerator FadeLightingCoroutine()
		{
			_light2D.intensity = 1;
			while (_light2D.intensity > 0)
			{
				_light2D.intensity -= Time.deltaTime / duration;
				yield return null;
			}

			_fadeLightingCoroutine = null;
		}

		private void FadeOnDeath()
		{
			if (_fadeLightingCoroutine == null) return;

			StopCoroutine(_fadeLightingCoroutine);
			duration = DurationOnAllDead;
			_fadeLightingCoroutine = StartCoroutine(FadeLightingCoroutine());
		}
	}
}