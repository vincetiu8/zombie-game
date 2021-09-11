using System;
using Mirror;
using UnityEngine;

namespace Game.Scripts.Weapons
{
    public class CollisionDamager : NetworkBehaviour
    {
        [SyncVar] public float damage;
        [SyncVar] public float damageCooldown;
        [SerializeField] private LayerMask layerMask;

        private float _cooldown;

        protected virtual void Update()
        {
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D other)
        {
            if (_cooldown > 0 || !Utils.IsInLayerMask(layerMask, other.gameObject.layer)) return;
            
            other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
            _cooldown = damageCooldown;
        }
    }
}