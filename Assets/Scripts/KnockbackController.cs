using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enemy;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class KnockbackController : MonoBehaviour
{
    [Description("Is multiplied with the amount of knockback target takes")] [SerializeField] [Range(0, 200)]
    protected float knockBackMultiplier;

    private Rigidbody2D _rigidbody2D;

    private float _cooldown;

    private bool _isStunned;

    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void TakeKnockBack(float angle, float amount)
    {
        float finalSpeed = amount * knockBackMultiplier;
        _rigidbody2D.velocity = TransformUtils.DegToVector2(angle) * finalSpeed;
    }

    public void TakeStun(int duration)
    {
        _cooldown += duration;
        //StartCoroutine(ShakeObject(duration));
    }

    private void Update()
    {
        if (_cooldown <= 0)
        {
            BeStunned(false);
            return;
        }
        _cooldown -= Time.deltaTime;
        BeStunned(true);
        //_rigidbody2D.velocity = Vector2.zero;
        //transform.position = transform.position;
        //if (transform.CompareTag("Player"))
    }
    
    // make method, in stun, out stun controleld by bool, same bool return

    private void BeStunned(bool stunned)
    {
        if (stunned == _isStunned) return;

        if (transform.GetComponent<ChaserAI>() != null)
        {
            transform.GetComponent<ChaserAI>().DisableMovement(stunned);
        }
      
        
        _isStunned = stunned;
    }
    
    private float shakeAmount = 0f;
    Vector2 startPosition;
    private bool duringShake;
    /// <summary>
    /// this is a terrible idea lmao
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator ShakeObject(int time)
    {
        int count = 0;
        //Changes position very fast when shot
            if (!duringShake)
            {
                startPosition = transform.position;
            }
            
            while (count < time)
            {
                duringShake = true;
                transform.position = startPosition + UnityEngine.Random.insideUnitCircle * shakeAmount;
                count += 1;
                //Debug.Log(count);
                duringShake = true;
                yield return new WaitForSeconds(0.01f);
            }
            transform.position = startPosition;
            duringShake = false;
    }
}
