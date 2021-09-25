using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    // A work-around to to make the shotgun shoot multiple bullets when the Gun script only instantiates a projectile once
    // This is script will instantiate multiple projectile instances before destroying itself
    public class ShotgunSpray : MonoBehaviour
    {
        private                  Rigidbody2D _rb;
        private                  Bullet      _bullet;
        [SerializeField] private int         pelletAmount;
        [SerializeField] private GameObject  pellet;
        [SerializeField] private float       spraySpread;

        void Start()
        {        
            Debug.Log("Firing shotgun");
            _rb      = transform.GetComponent<Rigidbody2D>();
            _bullet  = transform.GetComponent<Bullet>();

            for (int i = 0; i < pelletAmount; i++)
            {
                float spraySpreadAmount = Random.Range(-spraySpread, spraySpread);

                Quaternion currentRotation = transform.rotation;
                GameObject bulletClone = PhotonNetwork.Instantiate(pellet.name, transform.position, Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z + spraySpreadAmount));
            
                // Set the pass on attributes set by Gun script
                Vector2 sprayDirection = new Vector2(_rb.velocity.x * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad) - _rb.velocity.y * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad), _rb.velocity.x * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad) + _rb.velocity.y * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad));
                bulletClone.GetComponent<Rigidbody2D>().velocity = sprayDirection;
                bulletClone.GetComponent<Bullet>().damage        = _bullet.damage;
            }
            Destroy(gameObject);
        }

    }
}
