using System.Collections;
using System.Collections.Generic;
using Enemy;
using Photon.Pun;
using UnityEngine;
using Utils;

public class BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SummonZombies(10);
    }
    
    [SerializeField] private Object spawnedZombie;
    [SerializeField] private float spawnRadius = 3;

    [SerializeField] private float summonAmount;
    [SerializeField] private float summonAmountIncrementer;
    [SerializeField] private float summonDuration;


    private IEnumerator HandleSummon()
    {
        transform.GetComponent<ChaserAI>().DisableMovement(true);
        yield return new WaitForSeconds(summonDuration);
        // Probably play an animation here
        SummonZombies(Mathf.FloorToInt(summonAmount));
        summonAmount += summonAmountIncrementer;
        transform.GetComponent<ChaserAI>().DisableMovement(false);
    }
    
    /// <summary>
    /// Spawns zombies around the current gameobject in a circle
    /// </summary>
    /// <param name="amountOfObjectsToSpawn"></param>
    private void SummonZombies(int amountOfObjectsToSpawn)
    {
        float currentAngle = 0;
        for (int i = 0; i < amountOfObjectsToSpawn; i ++)
        {
            Vector2 offsetPosition = (Vector2) transform.position + TransformUtils.DegToVector2(currentAngle) * spawnRadius;
            currentAngle += 360 / amountOfObjectsToSpawn;

            PhotonNetwork.Instantiate(spawnedZombie.name, offsetPosition, Quaternion.identity);
        }
    }

    private void StunSpell(int amountToSpawn)
    {
        
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
