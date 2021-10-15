using UnityEngine;

namespace Interact
{
	public abstract class TimedInteractable : HoldInteractable
	{
		[Header("Time Settings")] [SerializeField] [Range(0.1f, 5f)]
		private float interactDuration;

		private float _remainingDuration;

		protected override void Start()
		{
			base.Start();
			ResetDuration();
		}

		protected virtual void Update()
		{
			if (!LocallyInteracting) return;

			_remainingDuration -= Time.deltaTime;

			if (_remainingDuration > 0) return;

			FinishInteraction();
		}

		public override float GetProgress()
		{
			return 1 - _remainingDuration / interactDuration;
		}

		protected void ResetDuration()
		{
			_remainingDuration = interactDuration;
		}

		public override void CancelInteraction()
		{
			ResetDuration();
			base.CancelInteraction();
		}

		protected override void FinishInteraction()
		{
			ResetDuration();
			base.FinishInteraction();
		}
	}
}