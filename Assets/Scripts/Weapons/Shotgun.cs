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
                GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z + spraySpreadAmount));
            
                // Set the pass on attributes set by Gun 
                Vector2 sprayDirection = new Vector2(direction.x * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad) - direction.y * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad), direction.x * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad) + direction.y * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad));
                bulletClone.GetComponent<Rigidbody2D>().velocity = sprayDirection;
                bulletClone.GetComponent<Bullet>().damage        = currentAttributes.damage;
            }
        }

    }
}
