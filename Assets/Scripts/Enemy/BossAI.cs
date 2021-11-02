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
    [Serializable] public struct BossAbility
    {
        public Action MethodToCall;
        public Func<IEnumerator> CoroutineToCall;
        public float castTime;
        public bool immobilizeWhilePerforming;
        //public Animation moveAnimation;
        public BossAbility(Action methodToCall,float castTime, bool immobilizeWhilePerforming)
        {
            this.MethodToCall = methodToCall;
            this.CoroutineToCall = null;
            this.castTime = castTime;
            this.immobilizeWhilePerforming = immobilizeWhilePerforming;
            //this.moveAnimation = null;
        }

        public BossAbility(Func<IEnumerator> routineToStart, float castTime, bool immobilizeWhilePerforming)
        {
            //void CoroutineToMethod() => StartCoroutine(routineToStart.Invoke());

            this.CoroutineToCall = routineToStart;
            this.MethodToCall = null;
            this.castTime = castTime;
            this.immobilizeWhilePerforming = immobilizeWhilePerforming;
            //this.moveAnimation = null;
        }
        //public abstract IEnumerator UseAbility();
    }
    public abstract class BossAI : MonoBehaviourPun
    {
        [SerializeField] private float minTimeBetweenActions;
        [SerializeField] private float maxTimeBetweenActions;

        [SerializeField] protected List<BossAbility> BossAbilities;

        private float _cooldown;

        private ChaserAI _chaserAI;

        protected void Start()
        {
            _chaserAI = transform.GetComponent<ChaserAI>();
            BossAbilities = new List<BossAbility>();
            DeclareBossMoves();
        }

        private void Update()
        {
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

            BossAbility ability = BossAbilities[Random.Range(0, BossAbilities.Count)];
            //BossMove move = BossMoves[1];
            if (ability.MethodToCall == null)
            {
                StartCoroutine(PerformAction(ability.CoroutineToCall,ability.castTime,ability.immobilizeWhilePerforming));
                return;
            }
            StartCoroutine(PerformAction(ability.MethodToCall,ability.castTime,ability.immobilizeWhilePerforming));
        }
        
        /// <summary>
        /// Every time a move is used, this is called
        /// </summary>
        /// <param name="bossMove"></param>
        /// <param name="castTime"></param>
        /// <param name="immobilizeWhilePerforming"></param>
        /// <returns></returns>
        private IEnumerator PerformAction(Action bossMove, float castTime, bool immobilizeWhilePerforming)
        {
            OnPerformAction();
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);
            
            float timePassed = 0;
            while (timePassed < castTime)
            {
                yield return new WaitForSeconds(0.1f);
                DuringPerformAction();
                timePassed += 0.1f;
            }
            
            // Probably play an animation here

            new Action(bossMove)();
            _chaserAI.DisableMovement(false);
            FinishPerformAction();
        }

        private IEnumerator PerformAction(Func<IEnumerator> routine, float castTime, bool immobilizeWhilePerforming)
        {
            OnPerformAction();
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);
            
            float timePassed = 0;
            while (timePassed < castTime)
            {
                yield return new WaitForSeconds(0.1f);
                DuringPerformAction();
                timePassed += 0.1f;
            }
            
            StartCoroutine(routine());
            _chaserAI.DisableMovement(false);
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
            List<Collider2D> targetsArray = Physics2D.OverlapCircleAll(transform.position, searchRadius, mask).ToList();
            
            // Removes boss from list if present
            Collider2D myCollider = transform.GetComponent<Collider2D>();
            if (targetsArray.Contains(myCollider)) targetsArray.Remove(myCollider);

            if (removeTargetsBehindObstacles)
                // Loops through all the objects found by overlapCircleAll
                foreach (Collider2D target in from target 
                                                  // Create a Raycast between the boss and the target in question
                                                  in targetsArray.ToList() let hits = Physics2D.RaycastAll
                                              (transform.position, 
                                                  target.transform.position - transform.position, 
                                                  Vector2.Distance(transform.position, target.transform.position)) 
                                              // Check if a wall is included in the array create by the Raycast (hits)
                                              where hits.Any(hit => hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles")) select target) 
                    // If there is a wall found, remove object from the target list
                    targetsArray.Remove(target);
                // All of this is just one line btw XD

                // Order list by how close players are to object
            return targetsArray.OrderBy(
                individualTarget => Vector2.Distance(this.transform.position, individualTarget.transform.position)).ToArray();
        }

        protected virtual void OnPerformAction()
        {
            photonView.RPC("RPCOnPerformAction", RpcTarget.All);
        }

        [PunRPC]
        protected abstract void RPCOnPerformAction();

        protected virtual void DuringPerformAction()
        {
            photonView.RPC("RPCDuringPerformAction", RpcTarget.All);
        }

        [PunRPC]
        protected abstract void RPCDuringPerformAction();

        protected virtual void FinishPerformAction()
        {
            photonView.RPC("RPCFinishPerformAction", RpcTarget.All);
        }

        [PunRPC]
        protected abstract void RPCFinishPerformAction();
    }
}
