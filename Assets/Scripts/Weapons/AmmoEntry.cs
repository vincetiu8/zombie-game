using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    //used in AmmoInventory
    [System.Serializable]
    public struct AmmoEntry
    {
        public string name;
        //name field included for editing convenience, not essential
        public int maxCapacity;
        public int currentStock;
    }
}
