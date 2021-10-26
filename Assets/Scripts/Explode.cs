using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private Animator _anim;
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    public IEnumerator ExplosionTrigger(int id)
    {
        _anim.SetTrigger(id);
        yield return new WaitForSeconds(2);
        Debug.Log("kaboom");
        Destroy(gameObject);
    }
}
