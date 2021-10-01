using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Collider2D))]
    public class MeleePoint : MonoBehaviour
    {
        public List<Collider2D> hitEnemies = new List<Collider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hitEnemies.Add(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                hitEnemies.Remove(collision);
            } 
        }
    }
}