using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_UI
{
    /// <summary>
    /// <para>UI component that handles information stored within the player gameObject.</para>
    /// <para>Ie. player: Health, ammo, weapon</para>
    /// </summary>
    public class PlayerStatistics : MonoBehaviour
    {
        [HideInInspector] public GameObject player;
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
}
