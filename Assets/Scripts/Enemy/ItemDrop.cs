using System;
using System.ComponentModel;
using UnityEngine;


[Serializable]
public class ItemDict : SerializableDictionary<int, GameObject>
{
}

public class ItemDrop : Health
{
    [Description("Item drops for this enemy")] [SerializeField]
    private ItemDict _itemDict;

    [Range(0, 101)] [SerializeField] private int _maxProb;

    private void Awake()
    {
        if (_itemDict == null)
        {
            _itemDict = new ItemDict();
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        DropSingleItem();
    }

    public void DropSingleItem()
    {
        int _prob = UnityEngine.Random.Range(1, _maxProb);

        foreach (var dictentry in _itemDict)
        {
            if (_prob <= dictentry.Key)
            {
                Instantiate(dictentry.Value, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}