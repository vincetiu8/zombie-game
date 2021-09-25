using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Photon.Pun;


public class Shotgun : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Bullet _bullet;
    [SerializeField] private int pelletAmount;
    [SerializeField] private GameObject pellet;
    [SerializeField] private float spraySpread;
    private float velocity;

    // Start is called before the first frame update
    void Start()
    {        
        Debug.Log("Firing shotgun");
        _rb = transform.GetComponent<Rigidbody2D>();
        _bullet = transform.GetComponent<Bullet>();
        velocity = _rb.velocity.magnitude;

        for (int i = 0; i < pelletAmount; i++)
        {
            float spraySpreadAmount = Random.Range(-spraySpread, spraySpread);

            GameObject bulletClone = PhotonNetwork.Instantiate(pellet.name, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + spraySpreadAmount));
            
            // Set the bullet's attributes
            Vector2 sprayDirection = new Vector2(_rb.velocity.x * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad) - _rb.velocity.y * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad), _rb.velocity.x * Mathf.Sin(spraySpreadAmount * Mathf.Deg2Rad) + _rb.velocity.y * Mathf.Cos(spraySpreadAmount * Mathf.Deg2Rad));
            bulletClone.GetComponent<Rigidbody2D>().velocity = sprayDirection;
            bulletClone.GetComponent<Bullet>().damage = _bullet.damage;
        }
        Destroy(gameObject);
    }

}
