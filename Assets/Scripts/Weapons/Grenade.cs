using System.Collections;
using Objects;
using UnityEngine;
using Photon.Pun;
using Utils;

namespace Weapons
{
    public class Grenade : BulletController
    {
        [Tooltip("How long until the grenade explodes")] [SerializeField]
        private float explosionDelay = 6f;
        
        [Tooltip("How big the grenade explosion is")] [SerializeField]
        private float blastRadius = 2f;

        [SerializeField] private GameObject explosionEffect;

        private float _explosionCountdown;
        private bool _hasExploded = false;
        private Explode _explode;
        private static readonly int ExplodeTrigger = Animator.StringToHash("detonate");

        private void Start()
        {
            _explosionCountdown = explosionDelay;
            _explode = GetComponentInChildren<Explode>();
        }

        private new void Update()
        {
            _explosionCountdown -= Time.deltaTime;
            if (!(_explosionCountdown <= 0f) || _hasExploded) return;
            Detonate();
            _hasExploded = true;
        }

        private void Detonate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

            foreach (var obj in colliders)
            {
                HealthController health = obj.GetComponent<HealthController>();
                KnockbackController knockbackController = obj.gameObject.GetComponent<KnockbackController>();
                float angle = TransformUtils.Vector2ToDeg(obj.transform.position - transform.position);

                if (health != null && knockbackController != null)
                {
                    knockbackController.TakeKnockBack(angle, knockBack);
                    health.ChangeHealth(-damage);
                    knockbackController.TakeStun(5);
                }
            }

            StartCoroutine(_explode.ExplosionTrigger(ExplodeTrigger));
            Destroy(gameObject);
        }
    }
}
