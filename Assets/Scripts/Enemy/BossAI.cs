using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
   
    public abstract class BossAI : MonoBehaviourPun
    {
        [SerializeField] private float minTimeBetweenActions;
        [SerializeField] private float maxTimeBetweenActions;

        [SerializeField] private Component bossAbilitiesReference;
        [SerializeField]private BossAbility[] BossAbilities;

        private float _cooldown;
        private ChaserAI _chaserAI;

        private bool _actionOngoing;

        protected void Start()
        {
            _chaserAI = transform.GetComponentInParent<ChaserAI>();
            BossAbilities = bossAbilitiesReference.GetComponentsInChildren<BossAbility>();
            DeclareBossMoves();
            Debug.Log(BossAbilities.Length);
        }

        private void Update()
        {
            if (_actionOngoing) return;

            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }
            
            Debug.Log("trying to choose an ability");
            AbilitySelectionLogic();
            _cooldown += Random.Range(minTimeBetweenActions, maxTimeBetweenActions);
        }

        /// <summary>
        /// Scripts that inherit from this will have their own methods they want to add to the list,
        /// since actions aren't serializable, this is my easy workaround :)
        /// </summary>
        protected abstract void DeclareBossMoves();

        protected virtual void AbilitySelectionLogic()
        {
            if (_chaserAI.GetTrackingPlayer() == null) return;

            // Very basic logic for now of just randomly choosing a move

            int abilityNo = Random.Range(0, BossAbilities.Length);
            DirectAbilityCall(abilityNo);
        }

        public void DirectAbilityCall(int abilityNumber)
        {
            Debug.Log("calling ability");
            BossAbility ability = BossAbilities[abilityNumber];
            _actionOngoing = true;
            StartCoroutine(ability.PerformAction());
        }

        protected virtual void OnAbilityFinish()
        {
            _actionOngoing = false;
        }
    }
}
