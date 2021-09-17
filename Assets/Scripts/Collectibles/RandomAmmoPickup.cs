using UnityEngine;

namespace Collectibles
{
    public class RandomAmmoPickup : AmmoPickup
    {
        [SerializeField] [Range(1, 50)] private int minAmt;
        [SerializeField] [Range(1, 50)] private int maxAmt;

        private void Awake()
        {
            dropAmount = Random.Range(minAmt, maxAmt);
        }
    }
}