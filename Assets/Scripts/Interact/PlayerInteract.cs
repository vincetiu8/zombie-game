using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    //List to keep track of how many interactables are in range, will always prioritise the most recent one
    private List<GameObject> interactPriorityList = new List<GameObject>(); //prioritiy lists are probably to complicated for such a simple thing surely

    //public GameObject interactIcon; will show an icon when an interactable object is nearby, but not implemented yet
    [SerializeField] private float interactRange = 3f;
    public void OpenInteractableIcon( GameObject interact)//maybe in the future, have a parameter that takes in icons to display in front of the player
    {
        interactPriorityList.Add(interact);
        Debug.Log("OpenInteractableIcon" + interact);
    }

    public void CloseInteractableIcon(GameObject interact)
    {
        interactPriorityList.Remove(interact);
        Debug.Log("CloseInteractableIcon" + interact);
    }

    public void CheckInteraction(InputAction.CallbackContext context)
    {
        Debug.Log(interactPriorityList);
        if (interactPriorityList.Count > 0)
        {
            interactPriorityList[interactPriorityList.Count - 1].GetComponent<Interactable>().Interact();
        }
        else
        {
            Debug.Log("No Interactables in range");
        }
    }
}