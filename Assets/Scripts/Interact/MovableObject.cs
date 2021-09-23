using UnityEngine;
using Weapons;

namespace Interact
{
    public class MovableObject : Interactable
    {
        private bool _isHolding;
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
            transform.SetParent(_isHolding ? player.transform.Find("PlayerObject").gameObject.transform : null);
            if (_isHolding)
            {
                player.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
                return;
            }
            player.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
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