using System;
using Mirror;
using UnityEngine;

namespace Game.Scripts
{
    public class Health : NetworkBehaviour
    {
        [SerializeField] private float initialHealth;

        [SyncVar] private float _health;

        private void Awake()
        {
            _health = initialHealth;
        }

        public float GetHealth()
        {
            return _health;
        }

        public int GetRoundedHealth()
        {
            return Mathf.RoundToInt(_health);
        }

        public void ChangeHealth(float change)
        {
            _health += change;

            if (_health > 0) return;

            OnDeath();
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}