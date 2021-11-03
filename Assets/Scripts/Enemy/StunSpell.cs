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
        private readonly GameObject _stunProjectile;
        private readonly float _delayPerSpell;

        public StunSpell(float castTime, bool immobilizeWhilePerforming, GameObject referenceObject, 
            GameObject stunProjectile, float delayPerSpell) : 
            base(castTime, immobilizeWhilePerforming, referenceObject)
        {
            _stunProjectile = stunProjectile;
            _delayPerSpell = delayPerSpell;
        }

        public override IEnumerator UseAbility()
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
                yield return new WaitForSeconds(_delayPerSpell);
            
                GameObject projectile =
                    PhotonNetwork.Instantiate(_stunProjectile.name, referenceObject.transform.position, Quaternion.Euler(new Vector3(0, 
                        Random.Range(0f, 0f), currentAngle)));
            
                currentAngle += 360 / amountToSpawn;

                // Sets target for each individual stun projectile
                if (targetPlayerNo > playerTargets.Length - 1) targetPlayerNo = 0;
                projectile.GetComponent<TrackerProjectile>().target = playerTargets[targetPlayerNo].transform;
                projectile.GetComponent<TrackerProjectile>().NecromancerAI = referenceObject.transform.GetComponent<NecromancerAI>();
                targetPlayerNo++;
            }
        }
    }
}