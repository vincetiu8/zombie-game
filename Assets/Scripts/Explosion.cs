using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Explosion : MonoBehaviourPun
{
    private Light2D _light;
    private Animator _anim;

    [SerializeField] private float explosionDelay;
    private Coroutine _explosionLightingCoroutine;
    private float _explosionCountdown;
    private bool _exploded = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _light = GetComponent<Light2D>();
        _light.intensity = 0;
        _anim.SetTrigger("detonate");
        photonView.RPC("RpcExplosionLighting", RpcTarget.All);
        _explosionCountdown = explosionDelay;
    }

    private void Update()
    {
        _explosionCountdown -= Time.deltaTime;
        if (_explosionCountdown > 0.1f || _exploded) return;
        Destroy(gameObject);
        _exploded = true;
        
    }
    
    [PunRPC]
    private void RpcExplosionLighting()
    {
        if(_explosionLightingCoroutine != null) StopCoroutine(_explosionLightingCoroutine);
        _explosionLightingCoroutine = StartCoroutine(ExplosionLighting());
    }
    
    private IEnumerator ExplosionLighting()
    {
        float threshold = explosionDelay / GetExplosionAnimLength();
        while (_light.intensity < threshold)
        {
            _light.intensity += Time.deltaTime / explosionDelay;
            yield return null;
        }

        _explosionLightingCoroutine = null;
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
