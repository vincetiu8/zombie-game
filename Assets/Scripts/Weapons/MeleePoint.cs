using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<Collider2D> _hitEnemies;

        private void Awake()
        {
            _hitEnemies = new List<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _hitEnemies.Add(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _hitEnemies.Remove(collision);   
            } 
        }
        
        /// <summary>
        /// Returns a list of enemies obtained from hitEnemies
        /// with all null (dead) enemies removed
        /// </summary>
        public List<Collider2D> GetEnemiesInCollider()
        {
            _hitEnemies.RemoveAll(enemy => enemy == null);
            return _hitEnemies.ToList();
        }
    }
}