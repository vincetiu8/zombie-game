using System.ComponentModel;
using UnityEngine;

namespace Enemy
{
    public class NecromancerAI : BossAI
    {
        [Header("Necromancer boss settings")]
        [Description("The amount of thins spawned in per summon (applies to both zombies and the stun projectile)")]
        [SerializeField] public float summonAmount;
        [SerializeField] public float summonAmountIncrementer;
        public float multiplierStacks = 1;

        public void IncreaseStackMultiplier(float amount)
        {
            if (multiplierStacks == 1) AbilitySelectionLogic();
            multiplierStacks += amount;
        }

        public override void OnAbilityFinish()
        {
            multiplierStacks = 1;
            summonAmount += summonAmountIncrementer;
            base.OnAbilityFinish();
        }
    }
}
