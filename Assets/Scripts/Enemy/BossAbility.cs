using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Ast;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Enemy
{
    [Serializable] public abstract class BossAbility : MonoBehaviourPun
    {
        [SerializeField] private float castTime;
        [SerializeField] private bool immobilizeWhilePerforming;
        //public Animation moveAnimation;
        
        protected Light2D _light2D;
        protected ChaserAI _chaserAI;
        private bool _duringPerformAction;
        protected GameObject referenceObject;

        private void Start()
        {
            referenceObject = transform.parent.gameObject;
            _light2D = transform.GetComponentInParent<Light2D>();
            _chaserAI = transform.GetComponentInParent<ChaserAI>();
        }

        private void Update()
        {
            if (_duringPerformAction) DuringPerformAction();
        }

        protected virtual void UseAbility()
        {
            StartCoroutine(AbilityCoroutine());
        }

        protected virtual IEnumerator AbilityCoroutine()
        {
            return null;
        }
        
        /// <summary>
        /// Every time a move is used, this is called
        /// </summary>
        /// <param name="bossMove"></param>
        /// <param name="routine"></param>
        /// <param name="castTime"></param>
        /// <param name="immobilizeWhilePerforming"></param>
        /// <returns></returns>
        // Gets called by the master client only
        public IEnumerator PerformAction()
        {
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);
            
            yield return new WaitForSeconds(castTime);
            
            UseAbility();
            _chaserAI.DisableMovement(false);
            
            GetComponentInParent<BossAI>().OnAbilityFinish();
        }

        // Gets called in a RPC
        public virtual void OnPerformAction()
        {
            _duringPerformAction = true;
        }
        
        protected virtual void DuringPerformAction()
        {
        }

        // Gets called in a RPC
        public virtual void FinishPerformAction()
        {
            _duringPerformAction = false;
        }
    }
}
