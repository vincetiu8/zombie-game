using UnityEngine;

namespace Game.Scripts.Weapons
{
    public class Bullet : CollisionDamager
    {
        [SerializeField] private float lifetime;

        private float _timeAlive;

        protected override void Update()
        {
            _timeAlive += Time.deltaTime;
            if (_timeAlive > lifetime)
            {
                Destroy(gameObject);
            }
        
            base.Update();
        }

        protected override void OnCollisionStay2D(Collision2D other)
        {
            base.OnCollisionStay2D(other);
        
            Destroy(gameObject);
        }
    }
}
