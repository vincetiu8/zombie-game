using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health_UI;

public class PlayerStatistics : MonoBehaviour
{
    public GameObject player;
    private HealthBarsLayout _healthBarsLayout;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => player != null);

        _healthBarsLayout = GetComponentInChildren<HealthBarsLayout>();
        Debug.Log(GetComponentInChildren<HealthBarsLayout>());

        _healthBarsLayout.AddHealthController(player.GetComponent<Health>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
