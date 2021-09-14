using System;

namespace Weapons
{
    public enum AmmoType { Primary, Heavy, Special }

    [Serializable]
    public struct AmmoEntry
    {
        public int maxCapacity;
        public int currentStock;
    }

    [Serializable]
    public class WeaponAttributes
    {
        public string description;
        public float damage;
        public float fireCooldown;
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