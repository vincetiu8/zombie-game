using UnityEngine;

namespace Interact
{
	public abstract class TimedInteractable : HoldInteractable
	{
		[Header("Time Settings")] [SerializeField] [Range(0.1f, 5f)]
		private float interactDuration;

		private float _remainingDuration;

		protected virtual void Update()
		{
			if (!LocallyInteracting) return;

			_remainingDuration -= Time.deltaTime;

			if (_remainingDuration > 0) return;

			FinishInteraction();
		}

		public override void StartInteraction()
		{
			base.StartInteraction();
			_remainingDuration = interactDuration;
		}
	}
}