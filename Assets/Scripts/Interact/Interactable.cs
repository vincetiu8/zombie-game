using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void Interact( GameObject player);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.GetComponent<PlayerInteract>().AddInteractableObject(gameObject); // Display the icon to let player know the object is interactable
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
            }
        }
    }
}
