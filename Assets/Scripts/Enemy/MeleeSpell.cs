using System;
using System.Collections;
using Photon.Pun;
using PlayerScripts;
using UnityEngine;
using Utils;
using Weapons;

namespace Enemy
{
    [Serializable] public class MeleeSpell : BossAbility
    {
        [SerializeField] private float meleeSpellDamage;
        [SerializeField] private float meleeSpellKnockback;
        [SerializeField] private SpriteRenderer animationSubstitude;
        [SerializeField] private float lungeSpeedMultiplier;
        [SerializeField] private MeleePoint meleePoint;

        protected override IEnumerator AbilityCoRoutine()
        {
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            float summonAmount = necromancerAI.summonAmount;
            float multiplierStacks = necromancerAI.multiplierStacks;
            
            
            int damage = Mathf.RoundToInt(meleeSpellDamage * multiplierStacks);
            float knockback = meleeSpellKnockback * multiplierStacks;
            
            Collider2D[] playerTargets = ListNearbyObjects(3 * multiplierStacks, "Players", true);
            
            if (playerTargets.Length == 0)
            {
                Debug.Log("No players in melee range, summoning zombies instead");
                //SummonZombies();
                yield break;
            }

            // Let the boss not collide with any zombies, and set it's speed to be faster for the lunge attack
            referenceObject.layer = LayerMask.NameToLayer("Objects");
            referenceObject.transform.GetComponent<ChaserAI>().ScaleAcceleration(lungeSpeedMultiplier * multiplierStacks);
            animationSubstitude.enabled = true;

            yield return new WaitForSeconds(1f);
            
            // End lunge attack
            referenceObject.transform.GetComponent<ChaserAI>().ResetAcceleration();
            animationSubstitude.enabled = false;
            referenceObject.layer = LayerMask.NameToLayer("Enemies");

            // Hit anything in the collider (the red box the boss made)
            foreach (Collider2D correctedPlayer in meleePoint.GetTargetsInCollider())
            {
                correctedPlayer.GetComponent<PlayerHealth>().ChangeHealth(-damage);
                
                if (correctedPlayer.GetComponent<KnockbackController>() == null) continue;
                
                float angle = TransformUtils.Vector2ToDeg(correctedPlayer.transform.position - referenceObject.transform.position);
                correctedPlayer.transform.GetComponent<KnockbackController>().TakeKnockBack(angle, knockback);
            }
        }
        
        // will have differnt animations and lighting effects for each move, so not doing any inheriting here
        /*[PunRPC]
        protected override void RPCOnPerformAction()
        {
            _light2D.enabled = true;
            base.RPCOnPerformAction();
        }*/

        protected override void DuringPerformAction()
        {
            _light2D.intensity = GetComponentInParent<NecromancerAI>().multiplierStacks;
        }

        /*[PunRPC]
        protected override void RPCFinishPerformAction()
        {
            _light2D.enabled = false;
            base.RPCFinishPerformAction();
        }*/

    }
}