using System;
using System.ComponentModel;
using UnityEngine;

namespace Weapons
{
	/// <summary>
	///     The attributes a weapon has.
	/// </summary>
	[Serializable]
	public class WeaponAttributes
	{
		[Description("The weapon's description")]
		public string description;

		[Description("The weapon's damage")] [Range(0, 1000)]public int damage;

        [Description("How much knockback the weapon create")] [Range(0, 100)]
        public float knockback;

		[Description("The number of seconds between weapon attacks")]
		public float fireCooldown;

		[Description("Whether the weapon automatically fires if the mouse is pressed")]
		public bool fullAuto;

		[Description("The Cooldown To Switch To This Weapon")] 
		public int switchingCooldown;

		public override string ToString()
		{
			return $"Description: {description}\n"
			       + $"Damage: {damage}\n"
			       + $"Fire Cooldown: {fireCooldown}\n"
			       + $"Full Auto: {fullAuto}\n";
		}
	}
}