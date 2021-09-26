using System.ComponentModel;
using UnityEngine;

namespace Collectibles
{
	/// <summary>
	///     Sets a random amount of ammo when initialized.
	///     Used when zombies drop ammo.
	/// </summary>
	public class RandomAmmoPickup : AmmoPickup
	{
		// These bounds override the dropAmount in the base class on awake
		[Header("Random Bounds")]
		[Description("The minimum amount of ammo to be dropped")]
		[SerializeField]
		[Range(1, 50)]
		private int minAmt;

		[Description("The maximum amount of ammo to be dropped")] [SerializeField] [Range(1, 50)]
		private int maxAmt;

		private void Awake()
		{
			dropAmount = Random.Range(minAmt, maxAmt);
		}
	}
}