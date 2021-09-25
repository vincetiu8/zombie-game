using System;
using System.ComponentModel;

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

		[Description("The weapon's damage")] public float damage;

		[Description("The number of seconds between weapon attacks")]
		public float fireCooldown;

		[Description("Whether the weapon automatically fires if the mouse is pressed")]
		public bool fullAuto;

		public override string ToString()
		{
			return $"Description: {description}\n"
			       + $"Damage: {damage}\n"
			       + $"Fire Cooldown: {fireCooldown}\n"
			       + $"Full Auto: {fullAuto}\n";
		}
	}
}