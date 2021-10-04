using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using Photon.Pun;

namespace Weapons
{
    /// <summary>
    /// A weapon that attacks within a
    /// small radius of the player
    /// </summary>
    public class MeleeAttack : Weapon
    {
        [Description("The weapon's attributes")] [SerializeField]
        private WeaponAttributes[] _levels;
        
        [SerializeField] private Animator animator;

        private MeleePoint meleePoint;

        protected override void Start()
        {
            base.Start();
            maxLevel = _levels.Length;
            currentAttributes = _levels[currentLevel];
            meleePoint = GetComponentInChildren<MeleePoint>();
        }

        protected override void Fire()
        {
            base.Fire();
            photonView.RPC("RpcMeleeAnimation", RpcTarget.All);

            foreach (Collider2D correctedEnemy in meleePoint.GetEnemiesInCollider())
            {
                correctedEnemy.GetComponent<Health>().ChangeHealth(-currentAttributes.damage);
            }
        }

        [PunRPC]
        private void RpcMeleeAnimation()
        {
            animator.SetTrigger("attack");
        }
    }
}