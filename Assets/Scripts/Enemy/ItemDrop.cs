using System;
using System.ComponentModel;
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
            base.OnDeath();
            DropSingleItem();
        }

        private void DropSingleItem()
        {
            int prob = UnityEngine.Random.Range(1, maxProb);

            foreach (var dictentry in itemDict)
            {
                if (prob > dictentry.Key) continue;
                Instantiate(dictentry.Value, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}