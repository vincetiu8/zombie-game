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
        [SerializeField] private SpriteRenderer animationSubstitute;
        [SerializeField] private float lungeSpeedMultiplier;
        [SerializeField] private MeleePoint meleePoint;

        protected override IEnumerator AbilityCoroutine()
        {
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            float multiplierStacks = necromancerAI.multiplierStacks;
            
            
            int damage = Mathf.RoundToInt(meleeSpellDamage * multiplierStacks);
            float knockback = meleeSpellKnockback * multiplierStacks;

            // Let the boss not collide with any zombies, and set it's speed to be faster for the lunge attack
            referenceObject.layer = LayerMask.NameToLayer("Objects");
            referenceObject.transform.GetComponent<ChaserAI>().ScaleAcceleration(lungeSpeedMultiplier * multiplierStacks);
            animationSubstitute.enabled = true;

            yield return new WaitForSeconds(1f);
            
            // End lunge attack
            referenceObject.transform.GetComponent<ChaserAI>().ResetAcceleration();
            animationSubstitute.enabled = false;
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
        
        protected override void DuringPerformActionClient()
        {
            _light2D.intensity = GetComponentInParent<NecromancerAI>().multiplierStacks;
        }
    }
}