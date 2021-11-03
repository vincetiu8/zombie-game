using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;
using Random = UnityEngine.Random;


namespace Enemy
{
    public class SummonZombies : BossAbility
    {
        public SummonZombies(float castTime, bool immobilizeWhilePerforming, GameObject referenceObject) : base(castTime, 
        immobilizeWhilePerforming, referenceObject) { }
        
        /// <summary>
        /// Spawns zombies around the current gameobject in a circle,
        /// the correct amount of space to split them between is also calculated here.
        /// </summary>
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
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            bool buffSpawnedZombies = necromancerAI.buffSpawnedZombies;
            float buffMultiplier = necromancerAI.buffMultiplier;
            float multiplierStacks = necromancerAI.multiplierStacks;


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
}