using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Photon.Pun;
using UnityEngine;
using Utils;
using Weapons;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BossAI : MonoBehaviour
{
    private List<Action> bossMoves;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        bossMoves = new List<Action>();
        
        bossMoves.Add(CalledSummonZombies);
        bossMoves.Add(CalledStunSpell);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minActionRate,maxActionRate));
            StartCoroutine(HandleSummon(bossMoves[Random.Range(0,bossMoves.Count)],true));
        }
    }

    [SerializeField] private int minActionRate;
    [SerializeField] private int maxActionRate;
    

    [SerializeField] private Object spawnedZombie;
    [SerializeField] private float spawnRadius = 3;

    [SerializeField] private float summonAmount;
    [SerializeField] private float summonAmountIncrementer;
    [SerializeField] private float summonDuration;

    
    [SerializeField] private Object stunProjectile;
    [SerializeField] private float delayPerSpell;
    
    private void CalledSummonZombies() => SummonZombies(Mathf.FloorToInt(summonAmount));
    private void CalledStunSpell() => StartCoroutine(StunSpell(Mathf.FloorToInt(summonAmount)));


    private IEnumerator HandleSummon(Action bossMove,bool immobalizeWhilePerforming)
    {
        if (immobalizeWhilePerforming)
        {
            transform.GetComponent<ChaserAI>().DisableMovement(true);
            yield return new WaitForSeconds(summonDuration);
        }
        // Probably play an animation here
        // shine some light?

        new Action(bossMove)();
        
        //StartCoroutine(StunSpell(Mathf.FloorToInt(summonAmount)));
        
        //SummonZombies(Mathf.FloorToInt(summonAmount));
        summonAmount += summonAmountIncrementer;
        
        transform.GetComponent<ChaserAI>().DisableMovement(false);
    }
    
    /// <summary>
    /// Spawns zombies around the current gameobject in a circle
    /// </summary>
    /// <param name="amountOfObjectsToSpawn"></param>
    private void SummonZombies(int amountOfObjectsToSpawn)
    {
        float currentAngle = Random.Range(0,360);
        for (int i = 0; i < amountOfObjectsToSpawn; i ++)
        {
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
        
        // Make a list of all close players
        LayerMask mask = LayerMask.GetMask("Players");
        Collider2D[] playerTargets = Physics2D.OverlapCircleAll(transform.position, 20f, mask);

        // Order list by how close players are to boss
        playerTargets = playerTargets.OrderBy(
            player => Vector2.Distance(this.transform.position, player.transform.position)).ToArray();

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
            targetPlayerNO++;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Bosses will have a set of attacks that they will do depending on a number of factors (for now determined by just distance)

    

    void Melee()
    {
        
    }

    void StunAttack()
    {
        
    }
}
