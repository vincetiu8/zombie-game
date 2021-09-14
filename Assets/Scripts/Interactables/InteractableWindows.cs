using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [RequireComponent(typeof(WindowController))]
    public class InteractableWindows : Interactable
    {
        private WindowController windowController;
        private PlayerMovement playerMovement;

        private float timer = 0.0f;
        [SerializeField] private float barricadeFixTime;

        private bool isFixing;

        public override void Interact()
        {
            timer += Time.deltaTime;
            if (timer > barricadeFixTime)
            {
                timer = timer - barricadeFixTime;
                windowController.ChangeHealth(-1);
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            //playerMovement = GetComponent
            windowController = GetComponent<WindowController>();
        }

    }
}
