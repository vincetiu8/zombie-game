using UnityEngine;
using Photon.Pun;


namespace Weapons
{
    public class Shotgun : Gun
    {
        [SerializeField] private int         pelletAmount;
        [SerializeField] private float       spraySpread;
        
        protected override void AdditionalFiringEffects(float direction)
        {
            for (int i = 0; i < pelletAmount; i++)
            {
                float firingAngle = direction + Random.Range(-spraySpread, spraySpread);
                base.AdditionalFiringEffects(firingAngle);
            }                
        }

    }
}
