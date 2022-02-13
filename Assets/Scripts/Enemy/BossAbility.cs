using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Enemy
{
    [Serializable]
    public abstract class BossAbility : MonoBehaviourPun
    {
        [SerializeField] private float castTime;
        [SerializeField] private bool immobilizeWhilePerforming;
        protected ChaserAI _chaserAI;

        private bool _duringPerformAction;
        //public Animation moveAnimation;

        protected Light2D _light2D;
        protected GameObject referenceObject;

        private void Start()
        {
            referenceObject = transform.parent.gameObject;
            _light2D = GetComponentInParent<Light2D>();
            _chaserAI = GetComponentInParent<ChaserAI>();
        }

        private void Update()
        {
            if (_duringPerformAction) DuringPerformActionClient();
        }

        public event EventHandler OnAbilityFinish;

        /// <summary>
        /// Every time a move is used, this is called
        /// </summary>
        /// <param name="bossMove"></param>
        /// <param name="routine"></param>
        /// <param name="castTime"></param>
        /// <param name="immobilizeWhilePerforming"></param>
        /// <returns></returns>
        // Gets called by the master client only
        public IEnumerator PerformActionMaster()
        {
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);

            yield return new WaitForSeconds(castTime);

            StartCoroutine(AbilityCoroutine());
            _chaserAI.DisableMovement(false);

            //GetComponentInParent<BossAI>().OnAbilityFinish();
            OnAbilityFinish?.Invoke(this, EventArgs.Empty);
        }

        public void OnPerformActionClient()
        {
            _duringPerformAction = true;
        }

        // Gets called in a RPC
        protected abstract IEnumerator AbilityCoroutine();

        protected virtual void DuringPerformActionClient()
        {
        }

        // Gets called in a RPC
        public virtual void FinishPerformActionClient()
        {
            _duringPerformAction = false;
        }
    }
}