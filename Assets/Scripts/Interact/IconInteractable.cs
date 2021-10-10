using UnityEngine;

namespace Interact
{
	public abstract class IconInteractable : Interactable
	{
		[Header("Icon Settings")] [SerializeField]
		private Canvas canvas;

		public override void OnClosestInteractable()
		{
			canvas.enabled = true;
		}

		public override void OnNotClosestInteractable()
		{
			canvas.enabled = false;
		}
	}
}