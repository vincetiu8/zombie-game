using UnityEngine;
using Weapons;

namespace Interact
{
    public class MovableObject : Interactable
    {
        private bool _isHolding;
        [SerializeField] private GameObject collier;
        private Collider2D[] _colList;

        private void Awake()
        {
            _colList = transform.GetComponentsInChildren<Collider2D>();
        }

        public override void Interact(GameObject player)
        {
            _isHolding = !_isHolding;
            player.GetComponent<WeaponsHandler>().PreventFire(_isHolding);
            SetAllCollidersStatus(!_isHolding);
            if (_isHolding)
            {
                gameObject.transform.SetParent (player.transform.Find("PlayerObject").gameObject.transform); 
                // Make box become part of the player, so it takes in rotation, etc

                player.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
                // Manually re-introduces the box into the player interaction list as the collider will not do so again since it got 
                // If this isn't done, pressing E will give a no "intractables in range"
            }
            else
            {
                gameObject.transform.SetParent (null);
                player.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
                // Manually removes the box from player interaction list as it seems OnTriggerExit2D doesn't get called properly
                // If this isn't done, player will be able to pick up the box from any distance away
            }
        }
        
        private void SetAllCollidersStatus(bool active)
        {
            foreach (Collider2D colliders in _colList)
            {
                colliders.enabled = active;
            }
        }
        
    }

}