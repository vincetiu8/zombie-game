using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class Lantern : MonoBehaviour {
    [SerializeField] private float duration;

    private Coroutine currentCoroutine;

    private Light2D _light2D;

    private void Start() {
        _light2D = GetComponent<Light2D>();
        currentCoroutine = StartCoroutine (Lighting(_light2D, false, duration));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine (Lighting(_light2D, false, duration));
        }                      
    }
    
    private IEnumerator Lighting(Light2D lightToFade, bool fadeIn, float duration) {
        float minLuminosity = 0;
        float maxLuminosity = 1; 
        float counter = 0f;
        float a, b;

        if (fadeIn) {
            a = minLuminosity;
            b = maxLuminosity;
        }
        else {
            a = maxLuminosity;
            b = minLuminosity;
        }

        float currentIntensity = _light2D.intensity;

        while (counter < duration) {
            counter += Time.deltaTime;
            _light2D.intensity = Mathf.Lerp(a, b, counter / duration);
            yield return null;
        }
    }
}
