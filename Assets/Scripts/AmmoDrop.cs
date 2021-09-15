using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] itemList; 
    private int itemIndex;
    private int dropNum;
    private Transform enemyPos;

    private void Start()
    {
        enemyPos = GetComponent<Transform>();
    }

    public void DropAmmo()
    {
        dropNum = Random.Range(0, 100);
        Debug.Log("dropNum is " + dropNum);

        if(dropNum >= 90)
        {
            itemIndex = 2;
            Instantiate(itemList[itemIndex], enemyPos.position, Quaternion.identity);
        }
        else if(dropNum < 90 && dropNum >= 60)
        {
            itemIndex = 1;
            Instantiate(itemList[itemIndex], enemyPos.position, Quaternion.identity);
        }
        else if(dropNum < 60)
        {
            itemIndex = 0;
            Instantiate(itemList[itemIndex], enemyPos.position, Quaternion.identity);
        }
    }
}
