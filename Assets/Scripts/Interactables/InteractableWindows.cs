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

        private float nextActionTime = 0.0f;
        [SerializeField] private float barricadeFixTime;

        private bool isFixing;

        public override void Interact()
        {
            Debug.Log("fixing window");
            if (Time.time > nextActionTime)
            {
                //Everything in here will be called every barricadeBreakTime interval
                nextActionTime = Time.time + barricadeFixTime;
                windowController.ChangeWindowState(1);
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
