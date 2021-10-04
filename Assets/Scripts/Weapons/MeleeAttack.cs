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
        
        private Animator _animator;

        private MeleePoint _meleePoint;

        protected override void Start()
        {
            base.Start();
            maxLevel = _levels.Length;
            currentAttributes = _levels[currentLevel];
            _meleePoint = GetComponentInChildren<MeleePoint>();
            _animator = GetComponent<Animator>();
        }

        protected override void Fire()
        {
            base.Fire();
            photonView.RPC("RpcMeleeAnimation", RpcTarget.All);

            foreach (Collider2D correctedEnemy in _meleePoint.GetEnemiesInCollider())
            {
                correctedEnemy.GetComponent<Health>().ChangeHealth(-currentAttributes.damage);
            }
        }

        [PunRPC]
        private void RpcMeleeAnimation()
        {
            _animator.SetTrigger("attack");
        }
    }
}