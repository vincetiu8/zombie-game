using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [RequireComponent(typeof(WindowController))]
    public class InteractableWindow : Interactable
    {
        private WindowController windowController;
        private PlayerMovement playerMovement;

        private float nextActionTime = 0.0f;
        [SerializeField] private float barricadeFixTime;

        private bool isFixing;
        private float _cooldown;

        public override void Interact()
        {
            if (_cooldown <= 0)
            {
                windowController.ChangeHealth(1);
                _cooldown += barricadeFixTime;
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            //playerMovement = GetComponent
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
