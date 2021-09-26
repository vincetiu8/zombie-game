using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;

namespace Player_UI
{
    /// <summary>
    /// <para>UI component that handles information stored within the player gameObject.</para>
    /// <para>Ie. player: Health, ammo, weapon</para>
    /// </summary>
    public class PlayerStatistics : MonoBehaviour
    {
        private GameObject _player;
        private HealthBarsLayout _healthBarsLayout;

        void Start()
        {
            _player = GameManager.instance.localPlayer;

            _healthBarsLayout = GetComponentInChildren<HealthBarsLayout>();
            Debug.Log(GetComponentInChildren<HealthBarsLayout>());

            _healthBarsLayout.AddHealthController(_player.GetComponent<Health>());
        }
    }
}
