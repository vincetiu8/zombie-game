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
        [SerializeField] public float summonAmount;
        [SerializeField] public float summonAmountIncrementer;
        public float multiplierStacks = 1;
        private Light2D _light2D;

        protected override void Start()
        {
            base.Start();
            _light2D = transform.GetComponent<Light2D>();
        }
        
        public void IncreaseStackMultiplier(float amount)
        {
            if (multiplierStacks == 1) AbilitySelectionLogic();
            multiplierStacks += amount;
        }

        protected override void AbilitySelectionLogic()
        {
            if (_chaserAI.GetTrackingPlayer() == null) return;

            // Summon zombies will always be a valid move
            List<int> possibleMoves = new List<int> {0};

            if (MiscUtils.CheckForObjectsInRadius(10, "Players", true, transform.gameObject)) possibleMoves.Add(1);
            if (MiscUtils.CheckForObjectsInRadius(3 * multiplierStacks, "Players", true, transform.gameObject)) possibleMoves.Add(2);
            
            DirectAbilityCall(possibleMoves);
        }

        public override void OnAbilityFinish()
        {
            multiplierStacks = 1;
            summonAmount += summonAmountIncrementer;
            base.OnAbilityFinish();
        }

        [PunRPC]
        protected override void RPCOnPerformAction()
        {
            _light2D.enabled = true;
            base.RPCOnPerformAction();
        }

        [PunRPC]
        protected override void RPCFinishPerformAction()
        {
            _light2D.enabled = false;
            base.RPCFinishPerformAction();
        }


    }
}
