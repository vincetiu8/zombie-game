using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

/// <summary>
/// Handles explosion effects
/// </summary>
public class Explosion : MonoBehaviourPun
{
    private Light2D _light;
    private Animator _anim;
    private Coroutine _explosionLightingCoroutine;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _light = GetComponent<Light2D>();
        _light.intensity = 0;
        _anim.SetTrigger("detonate");
        photonView.RPC("RpcExplosionLighting", RpcTarget.All);
    }

    [PunRPC]
    private void RpcExplosionLighting()
    {
        if(_explosionLightingCoroutine != null) StopCoroutine(_explosionLightingCoroutine);
        _explosionLightingCoroutine = StartCoroutine(ExplosionLighting());
    }
    
    private IEnumerator ExplosionLighting()
    {
        _light.intensity = 1;
        _light.pointLightOuterRadius = 2;
        while (_light.intensity > 0)
        {
            _light.intensity -= Time.deltaTime / 2;
            yield return null;
        }

        _explosionLightingCoroutine = null;
        Destroy(gameObject);
    }

    private float GetExplosionAnimLength()
    {
        float clipLength = 0;
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == "TempExplosion") clipLength = clip.length;
        }

        return clipLength;
    }
}
