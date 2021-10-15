using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enemy;
using Input;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;
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
    }
    
    // make method, in stun, out stun controleld by bool, same bool return

    private void BeStunned(bool stunned)
    {
        if (stunned == _isStunned) return;
        
        _isStunned = stunned;

        /*if (transform.CompareTag("Enemy"))
        {
            transform.GetComponent<ChaserAI>().DisableMovement(stunned);
        }*/

        switch (transform.tag)
        {
            case "Enemy":
                transform.GetComponent<ChaserAI>().DisableMovement(stunned);
                break;
            case "Player":
                MiscUtils.ActionMapOptions actionMap = stunned ? MiscUtils.ActionMapOptions.InAnimation : MiscUtils.ActionMapOptions.Game;
                MiscUtils.ToggleInput(actionMap, GameManager.instance.localPlayerInstance.GetComponent<PlayerInput>());
                break;
        }

    }
}
