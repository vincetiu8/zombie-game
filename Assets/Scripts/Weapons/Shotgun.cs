using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;


namespace Weapons
{
    public class Shotgun : Gun
    {
        [SerializeField] private int         _pelletAmount;
        [SerializeField] private float       _spraySpread;
        
        protected override void FireBullets()
        {
            float direction = firepoint.rotation.eulerAngles.z;
            for (int i = 0; i < _pelletAmount; i++)
            {
                float firingAngle = direction + Random.Range(-_spraySpread, _spraySpread);
                SpawnBullet(firingAngle);
            }                
        }

    }
}
