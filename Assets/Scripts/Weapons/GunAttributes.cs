using System;
using System.ComponentModel;

namespace Weapons
{
	/// <summary>
	/// AmmoType represents all the ammo types in the game.
	/// There are 3 kinds: primary, heavy and special.
	/// Each gun also has an AmmoType, representing the bullets it uses.
	/// </summary>
	public enum AmmoType
	{
		Primary,
		Heavy,
		Special,
		Grenade
	}

	/// <summary>
	/// An entry in the AmmoInventory. Each AmmoType has exactly 1 associated AmmoEntry.
	/// </summary>
	[Serializable]
	public struct AmmoEntry
	{
		[Description("The maximum number of bullets of this type the player can hold")]
		public int maxCapacity;

		[Description("The current number of bullets of this kind the player has")]
		public int currentStock;
	}

	/// <summary>
	///	Extra features a Gun has in addition to a normal weapon.
	/// </summary>
	[Serializable]
	public class GunAttributes : WeaponAttributes
	{
		[Description("The number of bullets that can be in the gun at once")]
		public int magazineSize;

		[Description("The time the gun takes to reload")]
		public float reloadTime;

		public override string ToString()
		{
			return base.ToString()
			       + $"Magazine Size: {magazineSize}\n"
			       + $"Reload Time: {reloadTime}\n";
		}
	}
}