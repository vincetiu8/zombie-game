using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;
using Weapons;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class BossAI : MonoBehaviour
    {
        [Header("Base Boss AI")]
        
        [SerializeField] private float minTimeBetweenActions;
        [SerializeField] private float maxTimeBetweenActions;
        [SerializeField] private float timeToPerformAction;

        protected List<Action> bossMoves;
        
        private IEnumerator Start()
        {
            bossMoves = new List<Action>();
            DeclareBossMoves();
            
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenActions,maxTimeBetweenActions));
                Debug.Log("trying to do something");
                StartCoroutine(PerformAction(bossMoves[Random.Range(0,bossMoves.Count)],true));
            }
        }

        /// <summary>
        /// Scripts that inherit from this will have their own methods they want to add to the list,
        /// since actions aren't serializable, this is my easy workaround :)
        /// </summary>
        protected virtual void DeclareBossMoves() { }
        
        private IEnumerator PerformAction(Action bossMove, bool immobilizeWhilePerforming)
        {
            OnPerformAction();
            if (immobilizeWhilePerforming)
            {
                transform.GetComponent<ChaserAI>().DisableMovement(true);
                yield return new WaitForSeconds(timeToPerformAction);
            }
            // Probably play an animation here

            new Action(bossMove)();
            transform.GetComponent<ChaserAI>().DisableMovement(false);
            FinishPeformAction();
        }
        
        protected virtual void OnPerformAction(){}
        protected virtual void FinishPeformAction(){}
    }
}
