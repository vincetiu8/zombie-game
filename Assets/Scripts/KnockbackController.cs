using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Utils;

public class KnockbackController : MonoBehaviour
{
    [Description("Is multiplied with the amount of knockback target takes")] [SerializeField] [Range(0, 200)]
    protected float knockBackMultiplier;

    private Rigidbody2D _rigidbody2D;

    private float _cooldown;

    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void TakeKnockBack(float angle, float amount)
    {
        float finalSpeed = amount * knockBackMultiplier;
        _rigidbody2D.velocity = TransformUtils.DegToVector2(angle) * finalSpeed;
    }

    public void TakeStun(float duration)
    {
        _cooldown += duration;
    }

    private void Update()
    {
        if (_cooldown <= 0) return;
        _cooldown -= Time.deltaTime;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
