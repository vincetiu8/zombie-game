using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Photon.Pun;
using PlasticPipe.PlasticProtocol.Client;
using PlayerScripts;
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
        private float _multiplierStacks = 1;

        [Header("Zombie spawn settings")]
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private float spawnRadius = 3;
        
        [Header("Stun projectile settings")]
        [SerializeField] private GameObject stunProjectile;
        [SerializeField] private float delayPerSpell;

        [Header("Melee AOE attack settings")] 
        [SerializeField] private float meleeSpellDamage;
        [SerializeField] private float meleeSpellKnockback;
        [SerializeField] private SpriteRenderer _animationSubstitude;
        [SerializeField] private float lungeSpeedMultiplier;

        [Header("Zombie buff settings")] 
        [SerializeField] private bool buffSpawnedZombies;
        [SerializeField] private float buffMultiplier;

        private MeleePoint _meleePoint;
        private Light2D _light2D;
        
        protected override void DeclareBossMoves()
        {
            _meleePoint = GetComponentInChildren<MeleePoint>();
            _light2D = transform.GetComponent<Light2D>();
            
            BossAbilities.Add(new BossAbility(SummonZombies, 3,true));
            BossAbilities.Add(new BossAbility(StunSpell, 2,true));
            BossAbilities.Add(new BossAbility(MeleeSpell, 0.5f,false));
        }

        // made these short methods so each move can be called easily
        //private void CalledSummonZombies() {}//=> SummonZombies(Mathf.FloorToInt(summonAmount * _multiplierStacks));
        //private void CalledStunSpell() {}//=> StartCoroutine(StunSpell());

        //private void CalledMeleeSpell() {}//=> StartCoroutine(MeleeSpell( ,))

        [PunRPC]
        protected override void RPCOnPerformAction()
        {
            _light2D.enabled = true;
            base.RPCOnPerformAction();
            //StartCoroutine(StunSpell());
        }

        protected override void DuringPerformAction()
        {
            _light2D.intensity = _multiplierStacks;
        }

        protected override void FinishPerformAction()
        {
            _multiplierStacks = 1;
            summonAmount += summonAmountIncrementer;
            base.FinishPerformAction();
        }

        [PunRPC]
        protected override void RPCFinishPerformAction()
        {
            _light2D.enabled = false;
            base.RPCFinishPerformAction();
        }



        /// <summary>
        /// Spawns zombies around the current gameobject in a circle,
        /// the correct amount of space to split them between is also calculated here.
        /// </summary>
        /// <param name="amountOfObjectsToSpawn"></param>
        private void SummonZombies()
        {
            int amountOfObjectsToSpawn = Mathf.FloorToInt(summonAmount * _multiplierStacks);
            
            if (BuffZombies()) return;
            float currentAngle = Random.Range(0,360);
            for (int i = 0; i < amountOfObjectsToSpawn; i ++)
            {
                // calculates a set amount of distance away from the object with equal intervals between points
                Vector2 offsetPosition = (Vector2) transform.position + TransformUtils.DegToVector2(currentAngle) * spawnRadius;
                currentAngle += 360 / amountOfObjectsToSpawn;

                PhotonNetwork.Instantiate(zombiePrefab.name, offsetPosition, Quaternion.identity);
            }
        }
        
        
        private IEnumerator StunSpell()
        {
            int amountToSpawn = Mathf.FloorToInt(summonAmount * _multiplierStacks);
            
            Collider2D[] playerTargets = ListNearbyObjects(10, "Players", true);

            if (playerTargets.Length == 0)
            {
                Debug.Log("Players all behind walls, summoning zombies instead");
                SummonZombies();
                yield break;
            }
            
            int targetPlayerNO = 0;
            float currentAngle = Random.Range(0,360);

            for (int i = 0; i < amountToSpawn; i ++)
            {
                yield return new WaitForSeconds(delayPerSpell);
            
                GameObject projectile =
                    PhotonNetwork.Instantiate(stunProjectile.name, transform.position, Quaternion.Euler(new Vector3(0, 
                        Random.Range(0f, 0f), currentAngle)));
            
                currentAngle += 360 / amountToSpawn;

                // Sets target for each individual stun projectile
                if (targetPlayerNO > playerTargets.Length - 1) targetPlayerNO = 0;
                projectile.GetComponent<TrackerProjectile>().target = playerTargets[targetPlayerNO].transform;
                projectile.GetComponent<TrackerProjectile>().NecromancerAI = transform.GetComponent<NecromancerAI>();
                targetPlayerNO++;
            }
        }

        private IEnumerator MeleeSpell()
        {
            int damage = Mathf.RoundToInt(meleeSpellDamage * _multiplierStacks);
            float knockback = meleeSpellKnockback * _multiplierStacks;
            
            Collider2D[] playerTargets = ListNearbyObjects(3 * _multiplierStacks, "Players", true);
            
            if (playerTargets.Length == 0)
            {
                Debug.Log("No players in melee range, summoning zombies instead");
                SummonZombies();
                yield break;
            }

            // Let the boss not collide with any zombies, and set it's speed to be faster for the lunge attack
            gameObject.layer = LayerMask.NameToLayer("Objects");
            transform.GetComponent<ChaserAI>().ScaleAcceleration(lungeSpeedMultiplier * _multiplierStacks);
            _animationSubstitude.enabled = true;

            yield return new WaitForSeconds(1f);
            
            // End lunge attack
            transform.GetComponent<ChaserAI>().ResetAcceleration();
            _animationSubstitude.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Enemies");

            // Hit anything in the collider (the red box the boss made)
            foreach (Collider2D correctedPlayer in _meleePoint.GetTargetsInCollider())
            {
                correctedPlayer.GetComponent<PlayerHealth>().ChangeHealth(-damage);
                
                if (correctedPlayer.GetComponent<KnockbackController>() == null) continue;
                
                float angle = TransformUtils.Vector2ToDeg(correctedPlayer.transform.position - transform.position);
                correctedPlayer.transform.GetComponent<KnockbackController>().TakeKnockBack(angle, knockback);
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
            
            ScaleZombiesStats(enemyTargets, buffMultiplier * _multiplierStacks);
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

        public void IncreaseStackMultiplier(float amount)
        {
            if (_multiplierStacks == 1)
            {
                AbilitySelectionLogic();
            }
            _multiplierStacks += amount;
        }
    }
}
