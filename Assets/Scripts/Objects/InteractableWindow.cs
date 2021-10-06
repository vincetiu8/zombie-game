using Interact;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     InteractableWindow implements the repair mechanic on windows.
	/// </summary>
	public class InteractableWindow : Interactable
	{
		[Description("How fast health is restored to the barricade")] [SerializeField] [Range(1, 100)]
		private int barricadeFixRate;
        //private                  float _cooldown;
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
			if (_cooldown > 0) _cooldown -= Time.deltaTime;
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

        public override void Interact()
        {
            // Don't allow window to be repaired if a zombie is currently attacking it
            if (_zombiesAtWindow > 0) return;

            // todo: fix this later to use hold interact
            _windowController.ChangeHealth(barricadeFixRate);
        }
        
        /*
		private void Update()
		{
            if (!_fixingWindow || _windowController.zombieAtWindow) return;

            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }
            if (!_fixingWindow || _windowController.zombieAtWindow) return;
            _windowController.ChangeHealth(1);
            // Cancels interaction if window fixed
            if (_windowController.IsWindowFixed()) CancelInteraction();
            _cooldown += barricadeFixTime;
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
        }*/
    }
}