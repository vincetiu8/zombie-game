using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
	/// <summary>
	///     Script to disable the editor light on play.
	///     This allows shadows to be removed in the editor for convenience.
	/// </summary>
	public class EditorLightDisabler : MonoBehaviour
	{
		private Light2D _light2D;

		private void Start()
		{
			_light2D = GetComponent<Light2D>();
			_light2D.intensity = 0;
		}
	}
}