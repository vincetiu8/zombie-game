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
        [Tooltip("The radius in which enemies are damaged")] [SerializeField]
        private float damageRadius;

        [Tooltip("How long enemies are stunned for")] [SerializeField]
        private int stunLength = 5;

        [SerializeField] private GameObject explosionEffect;

        private float _detonateCountdown;

        private void Start()
        {
            _detonateCountdown = lifetime;
        }

        private new void Update()
        {
            _detonateCountdown -= Time.deltaTime;
            if (!(_detonateCountdown <= 0f)) return;
            Detonate();
        }

        private void Detonate()
        {
            PhotonNetwork.Instantiate(explosionEffect.name, transform.position, Quaternion.identity);
            
            Collider2D[] damageColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

            foreach (var obj in damageColliders)
            {
                if (obj.gameObject.CompareTag("Player")) continue;
                HealthController health = obj.GetComponent<HealthController>();
                KnockbackController knockbackController = obj.gameObject.GetComponent<KnockbackController>();
                if (health != null) health.ChangeHealth(-damage);
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
        }
    }
}
