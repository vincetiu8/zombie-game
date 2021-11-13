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
        private BossAbility _currentAbility;
        private PhotonView _photonView;

        protected virtual void Start()
        {
            _photonView = GetComponentInParent<PhotonView>();
            _chaserAI = transform.GetComponentInParent<ChaserAI>();
            BossAbilities = bossAbilitiesReference.GetComponentsInChildren<BossAbility>();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            if (_actionOngoing) return;

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

        public void DirectAbilityCall(int abilityNumber)
        {
            Debug.Log("calling ability" + BossAbilities[abilityNumber]);
            _currentAbility = BossAbilities[abilityNumber];
            
            photonView.RPC("RPCOnPerformAction", RpcTarget.All);
            _actionOngoing = true;
            
            StartCoroutine(_currentAbility.PerformAction());
        }

        public void DirectAbilityCall(List<int> abilityNumbers)
        {
            int abilityNo = Random.Range(0, abilityNumbers.Count);
            DirectAbilityCall(abilityNo);
        }


        [PunRPC]
        protected virtual void RPCOnPerformAction()
        {
            _currentAbility.OnPerformAction();
        }
        
        [PunRPC]
        protected virtual void RPCFinishPerformAction()
        {
           _currentAbility.FinishPerformAction();
        }

        public virtual void OnAbilityFinish()
        {
            photonView.RPC("RPCFinishPerformAction", RpcTarget.All);
            _actionOngoing = false;
        }
    }
}
