using System;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;

namespace Enemy
{
    public class NecromancerAI : BossAI
    {
        [Header("Necromancer boss settings")]
        [Description("The amount of thins spawned in per summon (applies to both zombies and the stun projectile)")]
        [SerializeField]
        public float summonAmount;

        [SerializeField] public float summonAmountIncrementer;
        public float multiplierStacks = 1;
        private Light2D _light2D;

        protected override void Start()
        {
            base.Start();
            _light2D = transform.GetComponent<Light2D>();
        }

        // Resets attack cooldown when called while boss not currently using an ability, increases the multiplier if called during an ability
        public void IncreaseStackMultiplier(object sender, EventArgs e)
        {
            if (multiplierStacks == 1) AbilitySelectionLogic();
            multiplierStacks += summonAmountIncrementer;
        }

        protected override void AbilitySelectionLogic()
        {
            if (_chaserAI.GetTrackingPlayer() == null) return;

            // Summon zombies will always be a valid move
            List<int> possibleMoves = new List<int> {0};

            if (MiscUtils.CheckForObjectsInRadius(10, "Players", true, transform.gameObject)) possibleMoves.Add(1);
            if (MiscUtils.CheckForObjectsInRadius(4 * multiplierStacks, "Players", true, transform.gameObject))
                possibleMoves.Add(2);

            DirectAbilityCall(possibleMoves);
        }

        protected override void BossAbility_OnAbilityFinish(object sender, EventArgs e)
        {
            multiplierStacks = 1;
            summonAmount += summonAmountIncrementer;
            base.BossAbility_OnAbilityFinish(sender, e);
        }

        [PunRPC]
        protected override void RPCOnPerformAction(int abilityNumber)
        {
            _light2D.enabled = true;
            base.RPCOnPerformAction(abilityNumber);
        }

        [PunRPC]
        protected override void RPCFinishPerformAction(int abilityNumber)
        {
            _light2D.enabled = false;
            base.RPCFinishPerformAction(abilityNumber);
        }
    }
}