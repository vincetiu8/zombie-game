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
    [Serializable] public abstract class BossAbility
    {
        public float castTime;
        public bool immobilizeWhilePerforming;
        public GameObject referenceObject;
        //public Animation moveAnimation;
        
        protected BossAbility(float castTime, bool immobilizeWhilePerforming, GameObject referenceObject)
        {
            this.castTime = castTime;
            this.immobilizeWhilePerforming = immobilizeWhilePerforming;
            this.referenceObject = referenceObject;
            //this.moveAnimation = null;
        }

        public abstract IEnumerator UseAbility();
        
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

    public class Move1 : BossAbility
    {

        public Move1(float castTime, bool immobilizeWhilePerforming, GameObject referenceObject) : base(castTime, 
        immobilizeWhilePerforming, referenceObject) { }
        
        public override IEnumerator UseAbility()
        {
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            GameObject zombiePrefab = necromancerAI.zombiePrefab;
            float summonAmount = necromancerAI.summonAmount;
            float multiplierStacks = necromancerAI.multiplierStacks;
            float spawnRadius = necromancerAI.spawnRadius;
            
            int amountOfObjectsToSpawn = Mathf.FloorToInt(summonAmount * multiplierStacks);
            
            if (BuffZombies()) yield break;
            float currentAngle = Random.Range(0,360);
            for (int i = 0; i < amountOfObjectsToSpawn; i ++)
            {
                // calculates a set amount of distance away from the object with equal intervals between points
                Vector2 offsetPosition = (Vector2) referenceObject.transform.position 
                                         + TransformUtils.DegToVector2(currentAngle) * spawnRadius;
                
                currentAngle += 360 / amountOfObjectsToSpawn;

                PhotonNetwork.Instantiate(zombiePrefab.name, offsetPosition, Quaternion.identity);
            }
        }
        
        private bool BuffZombies()
        {
            Collider2D[] enemyTargets = ListNearbyObjects(5, "Enemies", false);
            
            if (enemyTargets.Length < 5) return false;

            if (!buffSpawnedZombies)
            {
                Debug.Log("BUFFING IS OFF: doing nothing");
                return true;
            }
            
            ScaleZombiesStats(enemyTargets, buffMultiplier * multiplierStacks);
            Debug.Log("BUFFING IS ON: Number of zombies exceeding threshold, buffing instead");
            return true;
        }
        
        private void ScaleZombiesStats(Collider2D[] listOfEnemies, float multiplier)
        {
            foreach (Collider2D enemy in listOfEnemies)
            {
                enemy.GetComponent<ChaserAI>().ScaleAcceleration(multiplier);
                enemy.GetComponent<EnemyHealth>().ScaleHealth(multiplier);
                enemy.GetComponent<KnockbackController>().ScaleKnockback(2-multiplier);
                enemy.GetComponentInChildren<AnimatedCollisionDamager>().ScaleDamage(multiplier);
                
                // Simple way to show how buff smth is for now
                Light2D enemyLight = enemy.GetComponent<Light2D>();
                if (enemyLight == null) return;
                enemyLight.enabled = true;
                enemyLight.intensity *= multiplier;
            }
        }
    }
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

            BossAbility ability = BossAbilities[Random.Range(0, BossAbilities.Count)];
            //BossMove move = BossMoves[1];
            StartCoroutine(PerformAction(ability.UseAbility(),ability.castTime,ability.immobilizeWhilePerforming));
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
            
            yield return new WaitForSeconds(castTime);

            // Probably play an animation here

            new Action(bossMove)();
            _chaserAI.DisableMovement(false);
            FinishPerformAction();
        }

        private IEnumerator PerformAction(IEnumerator routine, float castTime, bool immobilizeWhilePerforming)
        {
            OnPerformAction();
            if (immobilizeWhilePerforming) _chaserAI.DisableMovement(true);
            
            yield return new WaitForSeconds(castTime);
            
            StartCoroutine(routine);
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
