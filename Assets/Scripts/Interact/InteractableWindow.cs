using Levels;
using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(WindowController))]
    public class InteractableWindow : Interactable
    {
        private WindowController _windowController;

        [SerializeField] private float barricadeFixTime;

        private float _cooldown;

        public override void Interact(GameObject notUsedHaha)
        {
            if (_windowController.zombieAtWindow)
            {
                Debug.Log("Can't fix while zombie is at window");
                return;
            }

            if (_cooldown <= 0)
            {
                _windowController.ChangeHealth(1);
                _cooldown += barricadeFixTime;
            }
        }

        void Start()
        {
            _windowController = GetComponent<WindowController>();
        }

        private void Update()
        {
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
            }
        }
    }
}