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
        protected ChaserAI _chaserAI;

        private bool _actionOngoing;
        private int _currentAbilityNumber;
        private PhotonView _photonView;

        protected virtual void Start()
        {
            _photonView = GetComponentInParent<PhotonView>();
            _chaserAI = transform.GetComponentInParent<ChaserAI>();
            BossAbilities = bossAbilitiesReference.GetComponentsInChildren<BossAbility>();
        }

        private void Update()
        {
            if (!photonView.IsMine || _actionOngoing) return;

            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }
            
            Debug.Log("cooldown over, trying to choose an ability");
            AbilitySelectionLogic();
            _cooldown += Random.Range(minTimeBetweenActions, maxTimeBetweenActions);
        }

        protected virtual void AbilitySelectionLogic()
        {
            if (_chaserAI.GetTrackingPlayer() == null) return;

            // Very basic logic for now of just randomly choosing a move

            int abilityNo = Random.Range(0, BossAbilities.Length);
            DirectAbilityCall(abilityNo);
        }

        // Only gets run by the master client
        public void DirectAbilityCall(int abilityNumber)
        {
            _currentAbilityNumber = abilityNumber;
            Debug.Log("calling ability" + BossAbilities[_currentAbilityNumber]);
            
            photonView.RPC("RPCOnPerformAction", RpcTarget.All, _currentAbilityNumber);
            _actionOngoing = true;
            
            StartCoroutine(BossAbilities[_currentAbilityNumber].PerformActionMaster());
        }

        public void DirectAbilityCall(List<int> abilityNumbers)
        {
            int abilityNo = Random.Range(0, abilityNumbers.Count);
            DirectAbilityCall(abilityNo);
        }


        [PunRPC]
        protected virtual void RPCOnPerformAction(int abilityNumber)
        {
            BossAbilities[abilityNumber].OnPerformActionClient();
        }
        
        [PunRPC]
        protected virtual void RPCFinishPerformAction(int abilityNumber)
        {
            BossAbilities[abilityNumber].FinishPerformActionClient();
        }

        public virtual void OnAbilityFinish()
        {
            photonView.RPC("RPCFinishPerformAction", RpcTarget.All, _currentAbilityNumber);
            _actionOngoing = false;
        }
    }
}
