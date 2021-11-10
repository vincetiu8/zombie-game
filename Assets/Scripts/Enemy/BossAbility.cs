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

        /*protected BossAbility(float castTime, bool immobilizeWhilePerforming, GameObject referenceObject)
        {
            this.castTime = castTime;
            this.immobilizeWhilePerforming = immobilizeWhilePerforming;
            this.referenceObject = referenceObject;
            //this.moveAnimation = null;
        }*/

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


        // Abilities are ALWAYS Co-routines
        protected virtual void UseAbility()
        {
            StartCoroutine(AbilityCoRoutine());
        }

        protected virtual IEnumerator AbilityCoRoutine()
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
        public IEnumerator PerformAction()
        {
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);
            
            yield return new WaitForSeconds(castTime);
            
            UseAbility();
            _chaserAI.DisableMovement(false);
            
            GetComponentInParent<BossAI>().OnAbilityFinish();
        }

        public void OnPerformAction()
        {
            _light2D.enabled = true;
            _duringPerformAction = true;
        }
        
        protected virtual void DuringPerformAction()
        {
        }

        public void FinishPerformAction()
        {
            _light2D.enabled = false;
            _duringPerformAction = false;
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
            List<Collider2D> targetsArray = Physics2D.OverlapCircleAll(referenceObject.transform.position, searchRadius, 
            mask).ToList();
            
            // Removes boss from list if present
            Collider2D myCollider = referenceObject.GetComponent<Collider2D>();
            if (targetsArray.Contains(myCollider)) targetsArray.Remove(myCollider);

            if (removeTargetsBehindObstacles)
                // Loops through all the objects found by overlapCircleAll
                foreach (Collider2D target in from target 
                                                  // Create a Raycast between the boss and the target in question
                                                  in targetsArray.ToList() let hits = Physics2D.RaycastAll
                                              (referenceObject.transform.position, 
                                                  target.transform.position - referenceObject.transform.position, 
                                                  Vector2.Distance(referenceObject.transform.position, target.transform
                                                  .position)) 
                                              // Check if a wall is included in the array create by the Raycast (hits)
                                              where hits.Any(hit => hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles")) select target) 
                    // If there is a wall found, remove object from the target list
                    targetsArray.Remove(target);
            // All of this is just one line btw XD

            // Order list by how close players are to object
            return targetsArray.OrderBy(
                individualTarget => Vector2.Distance(referenceObject.transform.position, individualTarget.transform.position))
                .ToArray();
        }
    }
}