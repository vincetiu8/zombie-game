using UnityEngine;
using Interact;

namespace Collectibles
{
    public abstract class Collectible : Interactable
    {
        public override void Interact(GameObject player)
        {
            Pickup(player);
            Destroy(gameObject);
        }        

        protected abstract void Pickup(GameObject player);
    }
}