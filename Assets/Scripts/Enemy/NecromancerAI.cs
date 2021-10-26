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
        [SerializeField] private Object spawnedZombie;
        [SerializeField] private float spawnRadius = 3;
        
        [Header("Stun projectile settings")]
        [SerializeField] private Object stunProjectile;
        [SerializeField] private float delayPerSpell;

        [Header("Melee AOE attack settings")] 
        [SerializeField] private float meleeSpellDamage;
        [SerializeField] private float meleeSpellKnockback;
        [SerializeField] private SpriteRenderer _animationSubstitude;
        private MeleePoint _meleePoint;
        private Light2D _light2D;
        
        protected override void DeclareBossMoves()
        {
            _meleePoint = GetComponentInChildren<MeleePoint>();
            _light2D = transform.GetComponent<Light2D>();
            
            BossMoves.Add(new BossMove(CalledSummonZombies, 3,true));
            BossMoves.Add(new BossMove(CalledStunSpell, 2,true));
            BossMoves.Add(new BossMove(CalledMeleeSpell, 1,false));
        }

        private void CalledSummonZombies() => SummonZombies(Mathf.FloorToInt(summonAmount * _multiplierStacks));
        private void CalledStunSpell() => StartCoroutine(StunSpell(Mathf.FloorToInt(summonAmount * _multiplierStacks)));

        private void CalledMeleeSpell() => StartCoroutine(MeleeSpell( Mathf.RoundToInt(meleeSpellDamage * _multiplierStacks),
            meleeSpellKnockback * _multiplierStacks));

        protected override void OnPerformAction()
        {
            _light2D.enabled = true;
        }

        protected override void DuringPerformAction()
        {
            //transform.GetComponent<Light2D>().intensity = Mathf.MoveTowards(transform.GetComponent<Light2D>().intensity, _multiplierStacks,Time.deltaTime);
            _light2D.intensity = _multiplierStacks;
        }

        protected override void FinishPerformAction()
        {
            _multiplierStacks = 1;
            summonAmount += summonAmountIncrementer;
            _light2D.enabled = false;
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
            Collider2D[] playerTargets = ListNearbyObjects(20, "Players", true);

            if (playerTargets.Length == 0)
            {
                Debug.Log("Players all behind walls, summoning zombies instead");
                CalledSummonZombies();
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

        private IEnumerator MeleeSpell(int damage, float knockback)
        {
            Collider2D[] playerTargets = ListNearbyObjects(3 * _multiplierStacks, "Players", true);
            
            if (playerTargets.Length == 0)
            {
                Debug.Log("No players in melee range, summoning zombies instead");
                CalledSummonZombies();
                yield break;
            }

            // Let the boss not collide with any zombies, and set it's speed to be faster for the lunge attack
            gameObject.layer = LayerMask.NameToLayer("Objects");
            transform.GetComponent<ChaserAI>().SetAcceleration(6 * _multiplierStacks);
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

        public void IncreaseStackMultiplier(float amount)
        {
            if (_multiplierStacks == 1)
            {
                MoveSelectionLogic();
            }
            _multiplierStacks += amount;
        }
    }
}
