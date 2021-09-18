using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [Serializable]
    public class ItemDict : SerializableDictionary<int, GameObject>
    {
    }

    public class ItemDrop : Health
    {
        [Description("Item drops for this enemy")] [SerializeField]
        private ItemDict itemDict;

        [Range(0, 101)] [SerializeField] private int maxProb;

        private void Awake()
        {
            itemDict ??= new ItemDict();
        }

        protected override void OnDeath()
        {
            DropSingleItem();
            base.OnDeath();
        }

        private void DropSingleItem()
        {
            int prob = UnityEngine.Random.Range(1, maxProb);

            IEnumerable<KeyValuePair<int, GameObject>> droppedItems = itemDict.Where(dictEntry => prob <= dictEntry.Key);
            
            foreach (KeyValuePair<int, GameObject> dictEntry in droppedItems)
            {
                Instantiate(dictEntry.Value, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}