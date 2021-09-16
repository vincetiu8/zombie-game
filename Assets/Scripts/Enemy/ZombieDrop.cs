using System;
using System.ComponentModel;
using UnityEngine;

/*
This script is still being worked on
*/

[Serializable]
public class ItemDict : SerializableDictionary<int, GameObject>
{
}

public class ZombieDrop : MonoBehaviour
{
    [Description("Item drops for this enemy")]
    [SerializeField] private ItemDict _itemDict;
    System.Random _rand = new System.Random();
    private int _prob;
    private Transform _enemyPos;

    private void Awake()
    {
        if(_itemDict == null)
        {
            _itemDict = new ItemDict();
        }

        _enemyPos = GetComponent<Transform>();
    }

    public void DropSingleItem()
    {
        _prob = _rand.Next(1, 101);

        foreach (var dictentry in _itemDict)
        {
            if(_prob <= dictentry.Key)
            {
                Instantiate(dictentry.Value, _enemyPos.position, Quaternion.identity);
                break;
            }
        }
    }
}
