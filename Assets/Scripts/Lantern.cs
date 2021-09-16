using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lantern : MonoBehaviour {
    [SerializeField] private float duration;

    private Coroutine _currentCoroutine;

    private Light2D _light2D;

    private void Start() {
        _light2D = GetComponent<Light2D>();
        _currentCoroutine = StartCoroutine(Lighting());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

        _currentCoroutine = StartCoroutine(Lighting());
    }

    private IEnumerator Lighting() {
        _light2D.intensity = 1f;
        while (_light2D.intensity > 0) {
            _light2D.intensity -= Time.deltaTime / duration;
            yield return null;
        }
    }
}