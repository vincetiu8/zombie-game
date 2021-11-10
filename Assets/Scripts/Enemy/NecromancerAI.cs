using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Photon.Pun;
using PlasticPipe.PlasticProtocol.Client;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;
using Weapons;

namespace Enemy
{
    public class NecromancerAI : BossAI
    {
        [Header("Necromancer boss settings")]
        [Description("The amount of thins spawned in per summon (applies to both zombies and the stun projectile)")]
        [SerializeField] public float summonAmount;
        [SerializeField] public float summonAmountIncrementer;
        public float multiplierStacks = 1;

        [Header("Zombie spawn settings")]
        [SerializeField] public GameObject zombiePrefab;
        [SerializeField] public float spawnRadius = 3;
        
        [Header("Stun projectile settings")]
        [SerializeField] public GameObject stunProjectile;
        [SerializeField] public float delayPerSpell;

        [Header("Melee AOE attack settings")] 
        [SerializeField] public float meleeSpellDamage;
        [SerializeField] public float meleeSpellKnockback;
        [SerializeField] public SpriteRenderer animationSubstitude;

        [Header("Zombie buff settings")] 
        [SerializeField] public bool buffSpawnedZombies;
        [SerializeField] public float buffMultiplier;

        public MeleePoint meleePoint;
        private Light2D _light2D;
        
        protected override void DeclareBossMoves()
        {
            meleePoint = GetComponentInChildren<MeleePoint>();
            _light2D = transform.GetComponent<Light2D>();
          
            //BossAbilities.Add(new SummonZombies(3, true, gameObject, zombiePrefab, spawnRadius, buffSpawnedZombies, buffMultiplier));
            //BossAbilities.Add(new StunSpell(2, true, gameObject, stunProjectile, delayPerSpell));
            //.Add(new MeleeSpell(0.5f, false, gameObject, meleeSpellDamage, meleeSpellKnockback, animationSubstitude, lungeSpeedMultiplier, meleePoint));
        }

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
