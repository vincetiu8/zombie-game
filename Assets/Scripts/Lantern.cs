using System.Collections;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Interact;

public class Lantern : Interactable, IPunObservable {
    [SerializeField] private float duration;

    private Coroutine _fadeLightingCoroutine;
    private Light2D _light2D;

    private void Start() {
        _light2D = GetComponent<Light2D>();
        _light2D.intensity = 0;
    }

    public override void Interact(GameObject player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (_fadeLightingCoroutine != null) StopCoroutine(_fadeLightingCoroutine);

        photonView.RPC("RpcStartLightingCoroutine", RpcTarget.All);
    }

    [PunRPC]
    private void RpcStartLightingCoroutine()
    {
        _fadeLightingCoroutine = StartCoroutine(FadeLightingCoroutine());
    }

    private IEnumerator FadeLightingCoroutine() {
        _light2D.intensity = 1;
        while (_light2D.intensity > 0) {
            _light2D.intensity -= Time.deltaTime / duration;
            yield return null;
        }

        _fadeLightingCoroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}