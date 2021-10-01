using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Handles enemies coming into contact with
    /// the player's melee weapon
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class MeleePoint : MonoBehaviour
    {
        private List<Collider2D> hitEnemies = new List<Collider2D>();

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
        
        /// <summary>
        /// Returns a new list of enemies obtained from hitEnemies
        /// with all null (dead) enemies removed
        /// </summary>
        public List<Collider2D> GetEnemiesInCollider()
        {
            List<Collider2D> correctedEnemies = new List<Collider2D>();

            foreach (Collider2D enemy in hitEnemies)
            {
                if(enemy == null)
                {
                    hitEnemies.Remove(enemy);
                }
                else
                {
                    correctedEnemies.Add(enemy);
                }
            }

            return correctedEnemies;
        }
    }
}