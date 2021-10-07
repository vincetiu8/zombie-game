using System.ComponentModel;
using Interact;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	public class InteractableWindow : HoldInteractable
	{
		[Description("How fast health is restored to the barricade")] [SerializeField] [Range(1, 100)]
		private int barricadeFixRate;
        
        
        [Description("How much health is restored to the barricade each tick")] [SerializeField] [Range(1, 100)]
        private int barricadeFixAmount = 20;

        private                  float _cooldown;
        private bool _fixingWindow;

		private WindowController _windowController;
		private int              _zombiesAtWindow;

		protected override void Start()
		{
			base.Start();
			_windowController = GetComponentInChildren<WindowController>();
		}

        private void Update()
        {
            if (!_fixingWindow || _zombiesAtWindow > 0) return;
            
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }
            _windowController.ChangeHealth(barricadeFixAmount);
            // Cancels interaction if window fixed
            if (_windowController.IsWindowFixed()) CancelInteraction();
            _cooldown += barricadeFixRate;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            if (other.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;

            _zombiesAtWindow++;
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);

            if (other.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;

            _zombiesAtWindow--;
        }

        protected override void StartInteraction()
        {
            base.StartInteraction();
            _fixingWindow = true;
        }
        public override void CancelInteraction()
        {
            base.CancelInteraction();
            _fixingWindow = false;
        }
    }
}