using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [RequireComponent(typeof(WindowController))]
    public class InteractableWindow : Interactable
    {
        private WindowController windowController;

        [SerializeField] private float barricadeFixTime;

        private float _cooldown;

        public override void Interact()
        {
            if (windowController.zombieAtWindow)
            {
                Debug.Log("Can't fix while zombie is at window");
                return;
            }
            if (_cooldown <= 0)
            {
                windowController.ChangeHealth(1);
                _cooldown += barricadeFixTime;
            }
            
        }

        void Start()
        {
            windowController = GetComponent<WindowController>();
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
