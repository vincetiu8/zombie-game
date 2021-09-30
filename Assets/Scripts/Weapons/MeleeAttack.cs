using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace Weapons
{
    /// <summary>
    /// A weapon that attacks within a
    /// small radius of the player
    /// </summary>
    public class MeleeAttack : Weapon
    {
        [Header("Attack settings")]
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private Transform meleePoint;

        [SerializeField] private Animator animator;

        protected override void Fire()
        {
            base.Fire();
            animator.SetTrigger("attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, attackRange, mask);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("hit: " + enemy.name);
                enemy.gameObject.GetComponent<Health>().ChangeHealth(-currentAttributes.damage);
            }
        }
    }
}