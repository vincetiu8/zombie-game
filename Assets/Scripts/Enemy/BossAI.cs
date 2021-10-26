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
        [SerializeField] private float minTimeBetweenActions;
        [SerializeField] private float maxTimeBetweenActions;

        protected List<BossMove> BossMoves;
        [Serializable] protected struct BossMove
        {
            public Action MethodToCall;
            public int CastTime;
            public bool ImmobilizeWhilePerforming;
            public Animation MoveAnimation;
            public BossMove(Action methodToCall,int castTime, bool immobilizeWhilePerforming)
            {
                MethodToCall = methodToCall;
                CastTime = castTime;
                ImmobilizeWhilePerforming = immobilizeWhilePerforming;
                MoveAnimation = null;
            }

        }

        protected virtual IEnumerator Start()
        {
            BossMoves = new List<BossMove>();
            DeclareBossMoves();
            
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenActions,maxTimeBetweenActions));
                MoveSelectionLogic();
            }
        }

        /// <summary>
        /// Scripts that inherit from this will have their own methods they want to add to the list,
        /// since actions aren't serializable, this is my easy workaround :)
        /// </summary>
        protected virtual void DeclareBossMoves() { }

        protected virtual void MoveSelectionLogic()
        {
            // Very basic logic for now of just randomly choosing a move
            BossMove move = BossMoves[Random.Range(0, BossMoves.Count)];
            StartCoroutine(PerformAction(move.MethodToCall,move.CastTime,move.ImmobilizeWhilePerforming));
        }

        /// <summary>
        /// Every time a move is used, this is called
        /// </summary>
        /// <param name="bossMove"></param>
        /// <param name="castTime"></param>
        /// <param name="immobilizeWhilePerforming"></param>
        /// <returns></returns>
        protected IEnumerator PerformAction(Action bossMove, int castTime, bool immobilizeWhilePerforming)
        {
            OnPerformAction();
            if (immobilizeWhilePerforming) transform.GetComponent<ChaserAI>().DisableMovement(true);
            
            float timePassed = 0;
            while (timePassed < castTime)
            {
                yield return new WaitForSeconds(0.1f);
                DuringPerformAction();
                timePassed += 0.1f;
            }
            
            // Probably play an animation here

            new Action(bossMove)();
            transform.GetComponent<ChaserAI>().DisableMovement(false);
            FinishPerformAction();
        }
        
        /// <summary>
        /// Get objects around caller and orders it in order of closest to farthest
        /// </summary>
        /// <param name="searchRadius"></param>
        /// <param name="layerToSearch"></param>
        /// <param name="removeTargetsBehindObstacles"> If an object is behind a wall, should it still be included in the list</param>
        /// <returns></returns>
        protected Collider2D[] ListNearbyObjects(float searchRadius, string layerToSearch, bool removeTargetsBehindObstacles)
        {
            LayerMask mask = LayerMask.GetMask(layerToSearch);
            List<Collider2D> targets = Physics2D.OverlapCircleAll(transform.position, searchRadius, mask).ToList();

            if (removeTargetsBehindObstacles)
            {
                foreach (Collider2D target in targets.ToList())
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll
                        (transform.position, target.transform.position - transform.position, Vector2.Distance(transform.position, target.transform.position));
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles")) targets.Remove(target);
                        break;
                    }
                }
            }
            
            // Order list by how close players are to object
            return targets.OrderBy(
                individualTarget => Vector2.Distance(this.transform.position, individualTarget.transform.position)).ToArray();
        }
        
        protected virtual void OnPerformAction(){}
        protected virtual void DuringPerformAction(){}
        protected virtual void FinishPerformAction(){}
    }
}
