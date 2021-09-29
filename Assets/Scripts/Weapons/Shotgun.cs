using UnityEngine;
using Photon.Pun;


namespace Weapons
{
    public class Shotgun : Gun
    {
        [SerializeField] private int         pelletAmount;
        [SerializeField] private float       spraySpread;
        
        
        protected override void SpawnBullet(Vector2 direction)
        {
            
            for (int i = 0; i < pelletAmount; i++)
            {
                float spraySpreadAmount = Random.Range(-spraySpread, spraySpread);

                Quaternion currentRotation = transform.rotation;
                Quaternion bulletRotation =  Quaternion.Euler(0, 0, currentRotation.z + spraySpreadAmount);
                GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, bulletRotation);
                // Set the pass on attributes set by Gun 
                bulletClone.GetComponent<Rigidbody2D>().velocity = Utils.RotateVector2(direction, spraySpreadAmount);
                bulletClone.GetComponent<Bullet>().damage        = currentAttributes.damage;
            }
        }

    }
}
