using Enemy;
using UnityEngine;

namespace Weapons
{
    public class TrackerProjectile : MonoBehaviour
    // interiting from Bullet Controler doesn't work for some reason
    {
        public Transform target;
        [Range(0, 10)] [SerializeField] private int stunDuration;
        [Range(60, 360)] [SerializeField] private float angularAcceleration;
        [Range(0.5f, 10f)] [SerializeField] private float speed;

        [HideInInspector] public NecromancerAI NecromancerAI;

        private Rigidbody2D _rigidbody2D;

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!target) return;
            
            Vector2 direction = target.transform.position - transform.position;

            float rotateAmount = Vector3.Cross(direction.normalized, transform.up).z;
            
            _rigidbody2D.angularVelocity = -rotateAmount * angularAcceleration;

            _rigidbody2D.velocity = transform.up * speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("hit something");
            if (other.transform == target)
            {
                other.transform.GetComponent<KnockbackController>().TakeStun(stunDuration);
                NecromancerAI.IncreaseStackMultiplier(0.2f);
            }
            Destroy(gameObject);
        }
    }
}

