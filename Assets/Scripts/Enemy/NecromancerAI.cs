using System.Collections;
using System.ComponentModel;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;
using Weapons;

namespace Enemy
{
    public class NecromancerAI : BossAI
    {
        [Header("Necromancer boss settings")]
        [Description("The amount of thins spawned in per summon (applies to both zombies and the stun projectile)")]
        [SerializeField] private float summonAmount;
        [SerializeField] private float summonAmountIncrementer;
        
        [Header("Zombie spawn settings")]
        [SerializeField] private Object spawnedZombie;
        [SerializeField] private float spawnRadius = 3;
        
        [Header("Stun projectile settings")]
        [SerializeField] private Object stunProjectile;
        [SerializeField] private float delayPerSpell;

        protected override void DeclareBossMoves()
        {
            bossMoves.Add(CalledSummonZombies);
            bossMoves.Add(CalledStunSpell);
        }

        public void CalledSummonZombies() => SummonZombies(Mathf.FloorToInt(summonAmount));
        private void CalledStunSpell() => StartCoroutine(StunSpell(Mathf.FloorToInt(summonAmount)));

        protected override void OnPerformAction()
        {
            transform.GetComponent<Light2D>().enabled = true;
        }

        protected override void FinishPeformAction()
        {
            summonAmount += summonAmountIncrementer;
            transform.GetComponent<Light2D>().enabled = false;
        }


        /// <summary>
        /// Spawns zombies around the current gameobject in a circle,
        /// the correct amount of space to split them between is also calculated here.
        /// </summary>
        /// <param name="amountOfObjectsToSpawn"></param>
        private void SummonZombies(int amountOfObjectsToSpawn)
        {
            float currentAngle = Random.Range(0,360);
            for (int i = 0; i < amountOfObjectsToSpawn; i ++)
            {
                // calculates a set amount of distance away from the object with equal intervals between points
                Vector2 offsetPosition = (Vector2) transform.position + TransformUtils.DegToVector2(currentAngle) * spawnRadius;
                currentAngle += 360 / amountOfObjectsToSpawn;

                PhotonNetwork.Instantiate(spawnedZombie.name, offsetPosition, Quaternion.identity);
            }
        }
        
        
        private IEnumerator StunSpell(int amountToSpawn)
        {
            // find closest players, by makeing a gameobject list or smth
            // interate through list for each amount of stuns provided
            // 

            Collider2D[] playerTargets = ListNearbyObjects(20, "Players");
        
            if (playerTargets.Length == 0) yield break;
            int targetPlayerNO = 0;
            float currentAngle = Random.Range(0,360);

            for (int i = 0; i < amountToSpawn; i ++)
            {
                yield return new WaitForSeconds(delayPerSpell);
            
                GameObject projectile =
                    PhotonNetwork.Instantiate(stunProjectile.name, transform.position, Quaternion.Euler(new Vector3(0, 
                        Random.Range(0f, 0f), currentAngle)));
            
                currentAngle += 360 / amountToSpawn;

                // Sets target
                if (targetPlayerNO > playerTargets.Length - 1) targetPlayerNO = 0;
                projectile.GetComponent<TrackerProjectile>().target = playerTargets[targetPlayerNO].transform;
                projectile.GetComponent<TrackerProjectile>().NecromancerAI = transform.GetComponent<NecromancerAI>();
                targetPlayerNO++;
            }
        }

        private Collider2D[] ListNearbyObjects(int searchRadius, string layerToSearch)
        {
            LayerMask mask = LayerMask.GetMask(layerToSearch);
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, searchRadius, mask);

            // Order list by how close players are to object
            return targets.OrderBy(
                individualTarget => Vector2.Distance(this.transform.position, individualTarget.transform.position)).ToArray();
        }
    }
}
