using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interact
{
    public class PlayerInteract : MonoBehaviour
    {
        // List to keep track of how many interactables are in range, will be checked for closest object in CheckInteraction
        private readonly List<GameObject> _interactPriorityList = new List<GameObject>();

        public void AddInteractableObject( GameObject interact)// Maybe in the future, have a parameter that takes in icons to display in front of the player
        {
            _interactPriorityList.Add(interact);
        }

        public void RemoveInteractableObject(GameObject interact)
        {
            _interactPriorityList.Remove(interact);
        }

        public void CheckInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                if (_interactPriorityList.Count > 0)
                {
                    // Check which object in the list is closest to the player
                    float closestDistance = Mathf.Infinity;
                    GameObject closestObject = null;
                    foreach (GameObject obj in _interactPriorityList)
                    {
                        if (Vector2.Distance(obj.transform.position, transform.position) < closestDistance)
                        {
                            closestDistance = Vector2.Distance(obj.transform.position, transform.position);
                            closestObject = obj;
                            Debug.Log("closest object: " + closestObject);
                        }
                    }

                    closestObject.GetComponent<Interactable>().Interact(gameObject);
                }
                else
                {
                    Debug.Log("No Interactables in range");
                }
            }
        }
    }
}