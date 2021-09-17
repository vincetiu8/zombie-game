using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // List to keep track of how many interactables are in range, will be checked for closest object in CheckInteraction
    private List<GameObject> interactPriorityList = new List<GameObject>();

    public void AddInteractableObject( GameObject interact)// Maybe in the future, have a parameter that takes in icons to display in front of the player
    {
        interactPriorityList.Add(interact);
    }

    public void RemoveInteractableObject(GameObject interact)
    {
        interactPriorityList.Remove(interact);
    }

    public void CheckInteraction(InputAction.CallbackContext context)
    {
        if (interactPriorityList.Count > 0)
        {
            // Check which object in the list is closest to the player
            float _closestDistance = Mathf.Infinity;
            GameObject _closestObject = null;
            foreach (GameObject obj in interactPriorityList)
            {
                if (Vector2.Distance(obj.transform.position, transform.position) < _closestDistance) 
                {
                    _closestDistance = Vector2.Distance(obj.transform.position, transform.position);
                    _closestObject = obj;
                    Debug.Log("closest object: " + _closestObject);
                } 
            }
            _closestObject.GetComponent<Interactable>().Interact();
        }
        else
        {
            Debug.Log("No Interactables in range");
        }
    }
}