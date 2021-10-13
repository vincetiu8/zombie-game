using UnityEngine;

namespace Interact
{
	public abstract class IconInteractable : Interactable
	{
		[Header("Icon Settings")] [SerializeField]
		private Canvas canvas;

		public override void OnClosestInteractable()
		{
			if (canvas == null)
			{
				Debug.LogWarning("No canvas attached to IconInteractable, not toggling.");
				return;
			}

			canvas.enabled = true;
		}

		public override void OnNotClosestInteractable()
		{
			if (canvas == null)
			{
				Debug.LogWarning("No canvas attached to IconInteractable, not toggling.");
				return;
			}

			canvas.enabled = false;
		}
	}
}