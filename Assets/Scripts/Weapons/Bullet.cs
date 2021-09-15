using UnityEngine;

namespace Weapons
{
    public class Bullet : CollisionDamager
    {
        public float lifetime;

        private float _timeAlive;

        protected override void Update()
        {
            _timeAlive += Time.deltaTime;
            
            // Destroy the bullet if its outlived its lifetime
            if (_timeAlive > lifetime)
            {
                Destroy(gameObject);
            }
        
            base.Update();
        }

        // Destroy the bullet on a collision
        protected override void OnCollisionStay2D(Collision2D other)
        {
            Debug.Log(other);
            
            base.OnCollisionStay2D(other);
        
            Destroy(gameObject);
        }
    }
}
