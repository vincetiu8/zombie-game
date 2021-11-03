using System;
using System.Collections;
using PlayerScripts;
using UnityEngine;
using Utils;
using Weapons;

namespace Enemy
{
    [Serializable] public class MeleeSpell : BossAbility
    {
        private readonly float _meleeSpellDamage;
        private readonly float _meleeSpellKnockback;
        private readonly SpriteRenderer _animationSubstitude;
        private readonly float _lungeSpeedMultiplier;
        private readonly MeleePoint _meleePoint;

        public MeleeSpell(float castTime, bool immobilizeWhilePerforming, GameObject referenceObject, 
            float meleeSpellDamage, float meleeSpellKnockback, SpriteRenderer animationSubstitude, float lungeSpeedMultiplier, MeleePoint meleePoint) :
            base(castTime, immobilizeWhilePerforming, referenceObject)
        {
            _meleeSpellDamage = meleeSpellDamage;
            _meleeSpellKnockback = meleeSpellKnockback;
            _animationSubstitude = animationSubstitude;
            _lungeSpeedMultiplier = lungeSpeedMultiplier;
            _meleePoint = meleePoint;
        }

        public override IEnumerator UseAbility()
        {
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            float summonAmount = necromancerAI.summonAmount;
            float multiplierStacks = necromancerAI.multiplierStacks;
            
            
            int damage = Mathf.RoundToInt(_meleeSpellDamage * multiplierStacks);
            float knockback = _meleeSpellKnockback * multiplierStacks;
            
            Collider2D[] playerTargets = ListNearbyObjects(3 * multiplierStacks, "Players", true);
            
            if (playerTargets.Length == 0)
            {
                Debug.Log("No players in melee range, summoning zombies instead");
                //SummonZombies();
                yield break;
            }

            // Let the boss not collide with any zombies, and set it's speed to be faster for the lunge attack
            referenceObject.layer = LayerMask.NameToLayer("Objects");
            referenceObject.transform.GetComponent<ChaserAI>().ScaleAcceleration(_lungeSpeedMultiplier * multiplierStacks);
            _animationSubstitude.enabled = true;

            yield return new WaitForSeconds(1f);
            
            // End lunge attack
            referenceObject.transform.GetComponent<ChaserAI>().ResetAcceleration();
            _animationSubstitude.enabled = false;
            referenceObject.layer = LayerMask.NameToLayer("Enemies");

            // Hit anything in the collider (the red box the boss made)
            foreach (Collider2D correctedPlayer in _meleePoint.GetTargetsInCollider())
            {
                correctedPlayer.GetComponent<PlayerHealth>().ChangeHealth(-damage);
                
                if (correctedPlayer.GetComponent<KnockbackController>() == null) continue;
                
                float angle = TransformUtils.Vector2ToDeg(correctedPlayer.transform.position - referenceObject.transform.position);
                correctedPlayer.transform.GetComponent<KnockbackController>().TakeKnockBack(angle, knockback);
            }
        }
    }
}