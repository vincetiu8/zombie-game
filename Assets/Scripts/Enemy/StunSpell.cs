using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Utils;
using Weapons;
using Random = UnityEngine.Random;

namespace Enemy
{
    [Serializable] public class StunSpell : BossAbility
    {
        [SerializeField] private GameObject stunProjectile;
        [SerializeField] private float delayPerSpell;

        public override IEnumerator AbilityCoroutine()
        {
            NecromancerAI necromancerAI = referenceObject.GetComponent<NecromancerAI>();
            float summonAmount = necromancerAI.summonAmount;
            float multiplierStacks = necromancerAI.multiplierStacks;

            int amountToSpawn = Mathf.FloorToInt(summonAmount * multiplierStacks);
            
            Debug.Log(referenceObject);
            Collider2D[] playerTargets = MiscUtils.ListNearbyObjects(10, "Players", true, referenceObject);

            Debug.Log(playerTargets);
            
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

                Debug.Log(playerTargets[targetPlayerNo].transform);
                
                projectile.GetComponent<TrackerProjectile>().NecromancerAI = referenceObject.transform.GetComponent<NecromancerAI>();
                targetPlayerNo++;
            }
        }
        
        protected override void DuringPerformActionClient()
        {
            _light2D.intensity = GetComponentInParent<NecromancerAI>().multiplierStacks;
        }
    }
}