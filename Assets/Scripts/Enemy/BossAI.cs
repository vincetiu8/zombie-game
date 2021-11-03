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

        [SerializeField] protected List<BossAbility> BossAbilities;

        private float _cooldown;

        private ChaserAI _chaserAI;

        private bool _duringPerformAction;

        protected void Start()
        {
            _chaserAI = transform.GetComponent<ChaserAI>();
            BossAbilities = new List<BossAbility>();
            DeclareBossMoves();
        }

        private void Update()
        {
            if (_duringPerformAction) DuringPerformAction();
            
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }
            
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

            int abilityNo = Random.Range(0, BossAbilities.Count);
            DirectAbilityCall(abilityNo);
        }

        public void DirectAbilityCall(int abilityNumber)
        {
            BossAbility ability = BossAbilities[abilityNumber];
            StartCoroutine(PerformAction(ability.UseAbility(),ability.castTime,ability.immobilizeWhilePerforming));
        }

        /// <summary>
        /// Every time a move is used, this is called
        /// </summary>
        /// <param name="bossMove"></param>
        /// <param name="routine"></param>
        /// <param name="castTime"></param>
        /// <param name="immobilizeWhilePerforming"></param>
        /// <returns></returns>
        private IEnumerator PerformAction(IEnumerator routine, float castTime, bool immobilizeWhilePerforming)
        {
            OnPerformAction();
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);
            
            yield return new WaitForSeconds(castTime);
            
            StartCoroutine(routine);
            _chaserAI.DisableMovement(false);
            FinishPerformAction();
        }

        protected virtual void OnPerformAction()
        {
            photonView.RPC("RPCOnPerformAction", RpcTarget.All);
        }

        [PunRPC]
        protected virtual void RPCOnPerformAction()
        {
            _duringPerformAction = true;
        }

        protected abstract void DuringPerformAction();

        protected virtual void FinishPerformAction()
        {
            photonView.RPC("RPCFinishPerformAction", RpcTarget.All);
        }

        [PunRPC]
        protected virtual void RPCFinishPerformAction()
        {
            _duringPerformAction = false;
        }
    }
}
