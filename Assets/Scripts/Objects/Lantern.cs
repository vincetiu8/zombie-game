using System.Collections;
using System.ComponentModel;
using Interact;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
	// Lantern handles changing and syncing the lighting of the lanterns
	public class Lantern : Interactable
	{
		#region Variables

		[Header("Lantern Settings")] [Description("The duration the lantern stays on")] [SerializeField]
		private float duration;

		private Coroutine _fadeLightingCoroutine;
		private Light2D   _light2D;

		#endregion

		#region Methods

		private void Start()
		{
			_light2D = GetComponent<Light2D>();
			_light2D.intensity = 0;
		}

		public override void Interact(GameObject player)
		{
			if (!PhotonNetwork.IsMasterClient) return;

			if (_fadeLightingCoroutine != null) StopCoroutine(_fadeLightingCoroutine);

			photonView.RPC("RpcStartLightingCoroutine", RpcTarget.All);
		}

		[PunRPC]
		private void RpcStartLightingCoroutine()
		{
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

		#endregion
	}
}