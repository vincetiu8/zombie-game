using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public abstract class Collectible : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Pickup(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
    
    protected abstract void Pickup(GameObject _player);
}
