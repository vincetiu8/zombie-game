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
                
                // Create offset of original velocity vector by desired angle (didn't want to create one-time use variables)
                Vector2 sprayDirection = new Vector2(
                    // X direction, Shift vector by (current horizontal magnitude)*Cos(spray angle) subtracted by (current vertical magnitude)*Sin(spray angle)
                    direction.x * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad) - direction.y * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad),
                    // Y direction, Shift vector by (current horizontal magnitude)*Sin(spray angle) added by (current vertical magnitude)*Cos(spray angle)
                    direction.x * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad) + direction.y * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad)); 
                
                bulletClone.GetComponent<Rigidbody2D>().velocity = sprayDirection;
                bulletClone.GetComponent<Bullet>().damage        = currentAttributes.damage;
            }
        }

    }
}
