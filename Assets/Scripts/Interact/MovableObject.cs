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
            if (!_isHolding)
            {
                player.GetComponent<WeaponsHandler>().PreventFire(true);
                SetAllCollidersStatus(false);                
                // Prevents colliders getting buggy and also prevents other players from taking your box while you're holding it
                
                gameObject.transform.SetParent (player.transform.Find("PlayerObject").gameObject.transform); 
                // Make box become part of the player, so it takes in rotation, etc

                player.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
                // Manually re-introduces the box into the player interaction list as the collider will not do so again since it got 
                // If this isn't done, pressing E will give a no "intractables in range"
            }
            else
            {
                player.GetComponent<WeaponsHandler>().PreventFire(false);
                Debug.Log("Putting down box");
                SetAllCollidersStatus(true);
                gameObject.transform.SetParent (null);
                player.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
                // Manually removes the box from player interaction list as it seems OnTriggerExit2D doesn't get called properly
                // If this isn't done, player will be able to pick up the box from any distance away
            }
            _isHolding = !_isHolding;
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