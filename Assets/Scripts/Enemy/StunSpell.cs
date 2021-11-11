using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Enemy
{
    [Serializable] public class StunSpell : BossAbility
    {
        [SerializeField] private GameObject stunProjectile;
        [SerializeField] private float delayPerSpell;

        protected override IEnumerator AbilityCoRoutine()
        {
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            float summonAmount = necromancerAI.summonAmount;
            float multiplierStacks = necromancerAI.multiplierStacks;
            
            
            int amountToSpawn = Mathf.FloorToInt(summonAmount * multiplierStacks);
            
            Collider2D[] playerTargets = ListNearbyObjects(10, "Players", true);

            if (playerTargets.Length == 0)
            {
                Debug.Log("Players all behind walls, summoning zombies instead");
                necromancerAI.DirectAbilityCall(0);
                //SummonZombies();
                yield break;
            }
            
            int targetPlayerNo = 0;
            float currentAngle = Random.Range(0,360);

            for (int i = 0; i < amountToSpawn; i ++)
            {
                yield return new WaitForSeconds(delayPerSpell);
            
                GameObject projectile =
                    PhotonNetwork.Instantiate(stunProjectile.name, referenceObject.transform.position, Quaternion.Euler(new Vector3(0, 
                        Random.Range(0f, 0f), currentAngle)));
            
                currentAngle += 360 / amountToSpawn;

                // Sets target for each individual stun projectile
                if (targetPlayerNo > playerTargets.Length - 1) targetPlayerNo = 0;
                projectile.GetComponent<TrackerProjectile>().target = playerTargets[targetPlayerNo].transform;
                projectile.GetComponent<TrackerProjectile>().NecromancerAI = referenceObject.transform.GetComponent<NecromancerAI>();
                targetPlayerNo++;
            }
        }
        
        
        public override void OnPerformAction()
        {
            _light2D.enabled = true;
            base.OnPerformAction();
        }

        protected override void DuringPerformAction()
        {
            _light2D.intensity = GetComponentInParent<NecromancerAI>().multiplierStacks;
        }

        public override void FinishPerformAction()
        {
            _light2D.enabled = false;
            base.FinishPerformAction();
        }

    }
}