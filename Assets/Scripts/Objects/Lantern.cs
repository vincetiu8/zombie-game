using System;
using System.Collections;
using System.ComponentModel;
using Interact;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
	class Lantern : MonoBehaviour
	{
		/// <summary>
		///     Handles Flickering Lights Mod and changed values of the 2D Point Light respectively
		/// </summary>
		
		[Header("Light Flicker Settings")]
		[SerializeField] private bool flicker = false;
		[SerializeField] private bool flickerInUpdateMethod = true;

		[SerializeField] private float flickerIntervalTime;
		[SerializeField] private float lightIntensity;
		[SerializeField] private float flickerLightIntensity;

		private Light2D _light;

		private bool _canFlicker;

		private void Awake()
		{
			_light = GetComponent<Light2D>();
		}

		private void Update()
		{
			if (flickerInUpdateMethod == true && flicker == true)
			{
				_canFlicker = true;
				StartCoroutine(Flicker(flickerIntervalTime));
			}
		}

		//NOTE: I am putting this in a public void so if we want to make the lights flicker be triggered ALSO by another script like the game manager, we can do that.
		IEnumerator Flicker(float interval)
		{
			if (_canFlicker)
			{
				yield return new WaitForSeconds(interval);
				_light.intensity = flickerLightIntensity;
				
				yield return new WaitForSeconds(0.5f);
				_light.intensity = lightIntensity;

			}
			else Debug.Log($"{transform.name} CANNOT FLICKER");
		}
	}
}