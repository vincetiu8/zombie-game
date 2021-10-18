using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Utils;

public class BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Summon(10);
    }
    
    [SerializeField] private Object spawnedZombie;
    [SerializeField] private float spawnRadius = 3;

    [SerializeField] private float initialSummonAmount;
    [SerializeField] private float summonAmountIncrementer;

    /// <summary>
    /// Spawns zombies around the current gameobject in a circle
    /// </summary>
    /// <param name="amountOfObjectsToSpawn"></param>
    private void Summon(int amountOfObjectsToSpawn)
    {
        float currentAngle = 0;
        for (int i = 0; i < amountOfObjectsToSpawn; i ++)
        {
            Vector2 offsetPosition = (Vector2) transform.position + TransformUtils.DegToVector2(currentAngle) * spawnRadius;
            currentAngle += 360 / amountOfObjectsToSpawn;

            PhotonNetwork.Instantiate(spawnedZombie.name, offsetPosition, Quaternion.identity);
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
