using UnityEngine;
using Interact;

namespace Collectibles
{
    public abstract class Collectible : Interactable
    {
        public override void Interact(GameObject player)
        {
            if (!player.gameObject.CompareTag("Player")) return;
            Pickup(player.gameObject);
            Destroy(this.gameObject);
        }        

        protected abstract void Pickup(GameObject player);
    }
}