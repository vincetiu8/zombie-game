using System.ComponentModel;
using UnityEngine;

namespace Collectibles
{
	// RandomAmmoPickup sets a random amount of ammo when initialized
	// Used when zombies drop ammo
	public class RandomAmmoPickup : AmmoPickup
	{
		#region Methods

		private void Awake()
		{
			dropAmount = Random.Range(minAmt, maxAmt);
		}

		#endregion

		#region Variables

		// These bounds override the dropAmount in the base class on awake
		[Header("Random Bounds")]
		[Description("The minimum amount of ammo to be dropped")]
		[SerializeField]
		[Range(1, 50)]
		private int minAmt;

		[Description("The maximum amount of ammo to be dropped")] [SerializeField] [Range(1, 50)]
		private int maxAmt;

		#endregion
	}
}