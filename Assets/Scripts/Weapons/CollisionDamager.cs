using System.ComponentModel;
using UnityEngine;

namespace Weapons
{
    public class CollisionDamager : MonoBehaviour
    {
        [Description("The damage per attack")]
        public float damage;
        
        [Description("The cooldown between damage reductions")]
        public float damageCooldown;
        
        [Description("The layers the collision will affect")]
        [SerializeField] private LayerMask layerMask;

        // The current damage cooldown
        private float _cooldown;

        protected virtual void Update()
        {
            // Reduce cooldown by time
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D other)
        {
            // Make sure cooldown is complete and the collision is in the layermask to deal damage
            if (_cooldown > 0 || !Utils.IsInLayerMask(layerMask, other.gameObject.layer)) return;
            
            other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
            _cooldown = damageCooldown;
        }
    }
}