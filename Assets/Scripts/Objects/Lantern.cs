using System.Collections;
using System.ComponentModel;
using Interact;
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
		[Header("Lantern Settings")] [Description("The duration the lantern stays on")] [SerializeField]
		private float duration;

		private Coroutine _fadeLightingCoroutine;
		private Light2D   _light2D;

		protected override void Start()
		{
			base.Start();
			_light2D = GetComponent<Light2D>();
			_light2D.intensity = 0;
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
	}
}