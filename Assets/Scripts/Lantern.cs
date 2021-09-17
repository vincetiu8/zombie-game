using System.Collections;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lantern : MonoBehaviourPun, IPunObservable {
    [SerializeField] private float duration;

    private Coroutine _fadeLightingCoroutine;
    private Light2D _light2D;
    private float _currentIntensity;

    private void Start() {
        _light2D = GetComponent<Light2D>();
        _light2D.intensity = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (!collision.gameObject.CompareTag("Player")) return;
        if (_fadeLightingCoroutine != null) StopCoroutine(_fadeLightingCoroutine);

        _fadeLightingCoroutine = StartCoroutine(FadeLightingCoroutine());
    }

    private IEnumerator FadeLightingCoroutine() {
        _light2D.intensity = 1;
        while (_light2D.intensity > 0) {
            _light2D.intensity -= Time.deltaTime / duration;
            _currentIntensity = _light2D.intensity;
            yield return null;
        }

        _fadeLightingCoroutine = null;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_currentIntensity);
            return;
        }
        
        _light2D.intensity = (float)stream.ReceiveNext();
    }
}