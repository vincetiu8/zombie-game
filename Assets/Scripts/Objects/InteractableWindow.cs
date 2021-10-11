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
		private int barricadeFixDuration;
        
        
        [Description("How much health is restored to the barricade each tick")] [SerializeField] [Range(1, 100)]
        private int barricadeFixAmount = 20;

        private                  float _cooldown;
        //private bool _fixingWindow;

		private WindowController _windowController;
		private int              _zombiesAtWindow;

		protected override void Start()
		{
			base.Start();
			_windowController = GetComponentInChildren<WindowController>();
		}

        private void Update()
        {
            if (!_performInteraction || _zombiesAtWindow > 0) return;

            if (_windowController.IsWindowFixed())
            {
                CancelInteraction();
                return;
            }

            // Cooldown only decreases when player is actively fixing the window
            // Done to prevent players from tapping E every cooldown 0 to insta fix a barricade while being able to shoot their guns
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }
            _windowController.ChangeHealth(barricadeFixAmount);
            _cooldown += barricadeFixDuration;
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
        public override void CancelInteraction()
        {
            // Done to prevent player from 99ing window fixes
            _cooldown = barricadeFixDuration;
            
            base.CancelInteraction();
            //_fixingWindow = false;
        }
    }
}