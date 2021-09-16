using System;
using System.ComponentModel;
using UnityEngine;

/*
This script can be attached to any zombie enemy as a component
and using the serialized dictionary we can specify drops
and drop rates
*/
[Serializable]
public class ItemDict : SerializableDictionary<GameObject, int>
{
}

public class ZombieDrop : DropManager
{
    [Description("Item drops for this enemy")]
    [SerializeField] private ItemDict _itemDict;

    private void Awake()
    {
        if(_itemDict == null)
        {
            _itemDict = new ItemDict();
        }
    }
}
