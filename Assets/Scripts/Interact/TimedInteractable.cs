using UnityEngine;

namespace Interact
{
	public abstract class TimedInteractable : HoldInteractable
	{
		[Header("Time Settings")] [SerializeField] [Range(0.1f, 5f)]
		private float interactDuration;

		private float _elapsedDuration;

		private void Update()
		{
			if (!LocallyInteracting) return;

			_elapsedDuration -= Time.deltaTime;

			if (_elapsedDuration > 0) return;

			FinishInteraction();
		}

		public override void StartInteraction()
		{
			base.StartInteraction();
			_elapsedDuration = interactDuration;
		}
	}
}