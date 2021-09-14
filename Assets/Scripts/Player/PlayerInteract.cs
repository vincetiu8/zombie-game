using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    //public GameObject interactIcon; will show an icon when an interactable object is nearby, but not implemented yet
    [SerializeField] private float interactRange = 3f;
    public void OpenInteractableIcon()//maybe in the future, have a parameter that takes in icons to display in front of the player
    {
        //interactIcon.SetActive(true);
        Debug.Log("OpenInteractableIcon");
    }

    public void CloseInteractableIcon()
    {
        //interactIcon.SetActive(true);
        Debug.Log("CloseInteractableIcon");
    }

    public void CheckInteraction(InputAction.CallbackContext context)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interactRange, new Vector2 (0,0));

        if(hits.Length > 0)
        {
            foreach(RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    Debug.Log("Interactable found");
                    return;
                }
            }
        }
    }
}