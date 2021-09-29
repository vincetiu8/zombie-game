using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class MeleeAttack : Gun
    {
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private float meleeDamage;

        protected override void Fire()
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firepoint.position, attackRange, mask);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("hit: " + enemy.name);
                enemy.gameObject.GetComponent<Health>().ChangeHealth(-meleeDamage);
            }
        }
    }
}