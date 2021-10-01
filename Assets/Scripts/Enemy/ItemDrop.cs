using System;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
	[Description("A mapping of a percentage to an object. If the rolled percentage is lower, the object is dropped.")]
	[Serializable]
	public class ItemDict : SerializableDictionary<int, GameObject>
	{
	}

	/// <summary>
	///     An extension of the Health script that drops an item when the object is destroyed
	/// </summary>
	public class ItemDrop : Health
	{
		[Header("Item Drops")] [Description("Item drops for this enemy")] [SerializeField]
		private ItemDict itemDict;

		private void Awake()
		{
			if (itemDict == null || itemDict.Count == 0)
				Debug.LogWarning("No items to be dropped, prefer using the base Health script instead");
		}

		protected override void OnDeath()
		{
			DropSingleItem();
			base.OnDeath();
		}

		private void DropSingleItem()
		{
			// Calculate the probability
			int prob = Random.Range(0, 100);
			int minHit = 101;
			GameObject minItem = null;

			// Loop through all items and find the most improbably item that could be dropped
			foreach (KeyValuePair<int, GameObject> dictEntry in itemDict)
			{
				if (dictEntry.Key > prob || dictEntry.Key > minHit) continue;

				minHit = dictEntry.Key;
				minItem = dictEntry.Value;
			}

			// Don't instantiate if nothing was rolled
			if (minItem == null) return;

			PhotonNetwork.Instantiate(minItem.name, transform.position, Quaternion.identity);
		}
	}
}