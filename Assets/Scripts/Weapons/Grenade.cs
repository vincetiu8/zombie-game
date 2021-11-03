using Objects;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;
using Utils;

namespace Weapons
{
    /// <summary>
    /// Handles grenade projectile logic
    /// </summary>
    public class Grenade : BulletController
    {
        [Tooltip("How long until the grenade explodes")] [SerializeField]
        private float detonateDelay = 3f;
        
        [Tooltip("The radius in which enemies are damaged")] [SerializeField]
        private float damageRadius; 

        [Tooltip("The radius in which enemies are stunned")] [SerializeField]
        private float stunRadius;

        [Tooltip("How long enemies are stunned for")] [SerializeField]
        private int stunLength = 5;

        [SerializeField] private GameObject explosionEffect;

        private float _detonateCountdown;
        private bool _hasExploded = false;

        private void Start()
        {
            _detonateCountdown = detonateDelay;
        }

        private new void Update()
        {
            _detonateCountdown -= Time.deltaTime;
            if (!(_detonateCountdown <= 0f) || _hasExploded) return;
            Detonate();
            _hasExploded = true;
        }

        private void Detonate()
        {
            PhotonNetwork.Instantiate(explosionEffect.name, transform.position, Quaternion.identity);
            
            Collider2D[] damageColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
            Collider2D[] stunColliders = Physics2D.OverlapCircleAll(transform.position, stunRadius);

            foreach (var obj in damageColliders)
            {
                if (obj.gameObject.CompareTag("Player")) continue;
                HealthController health = obj.GetComponent<HealthController>();
                if (health != null)
                {
                    health.ChangeHealth(-damage);
                }
            }

            foreach (var obj in stunColliders)
            {
                if (obj.gameObject.CompareTag("Player")) continue;
                KnockbackController knockbackController = obj.gameObject.GetComponent<KnockbackController>();
                if (knockbackController != null)
                {
                    float angle = TransformUtils.Vector2ToDeg(obj.transform.position - transform.position);
                    knockbackController.TakeKnockBack(angle, knockBack);
                    knockbackController.TakeStun(stunLength);
                }
            }
            
            Destroy(gameObject);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, damageRadius);
            Gizmos.DrawWireSphere(transform.position, stunRadius);
        }
    }
}