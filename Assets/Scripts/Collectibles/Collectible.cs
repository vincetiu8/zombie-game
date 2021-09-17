using UnityEngine;

namespace Collectibles
{
    public abstract class Collectible : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            Pickup(collision.gameObject);
            Destroy(this.gameObject);
        }

        protected abstract void Pickup(GameObject player);
    }
}