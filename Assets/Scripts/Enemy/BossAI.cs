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
        private BossAbility _currentAbility;

        protected void Start()
        {
            _chaserAI = transform.GetComponentInParent<ChaserAI>();
            BossAbilities = bossAbilitiesReference.GetComponentsInChildren<BossAbility>();
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
            _currentAbility = BossAbilities[abilityNumber];
            
            photonView.RPC("RPCOnPerformAction", RpcTarget.All);
            _actionOngoing = true;
            
            StartCoroutine(_currentAbility.PerformAction());
        }


        [PunRPC]
        protected void RPCOnPerformAction()
        {
            _currentAbility.OnPerformAction();
        }
        
        [PunRPC]
        protected void RPCFinishPerformAction()
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
