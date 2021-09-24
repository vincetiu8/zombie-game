using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Player_UI
{
    /// <summary>
    /// Handles how healthbar displaying. Supports multiple health bars, will be sorted out in the usual Vertical Layout Group fassion.
    /// </summary>
    public class HealthBarsLayout : MonoBehaviour
    {
        [HideInInspector] public HealthBarsLayout healthBarsLayout;
        
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private int maxBars;
        private List<HealthBar> _healthBars;
        private SortedList<int, Health> _healthControllers;

        private void Awake()
        {

            if (healthBarsLayout)
            {
                Destroy(this);
                return;
            }

            healthBarsLayout = this;
            _healthControllers = new SortedList<int, Health>();
            _healthBars = new List<HealthBar>();
        }

        private void Start()
        {
            // Check for all healthbars that require displaying, instantiates that amount of healthbars
            for (int i = 0; i < maxBars; i++)
            {
                GameObject healthBar = Instantiate(healthBarPrefab, transform);
                _healthBars[i] = healthBar.GetComponent<HealthBar>();
                healthBar.SetActive(false);
            }
        }

        /// <summary>
        /// Creates a new health bar in the <c>_healthBars</c> list as well as instantiates it in the UI.
        /// </summary>
        /// <param name="healthController"> takes in the <c>Health</c> component of an object</param>
        /// <example><code>_healthBarsLayout.AddHealthController(player.GetComponent>Health>())</code></example>
        public void AddHealthController(Health healthController)
        {
            _healthControllers.Add(Mathf.RoundToInt(healthController.GetInitialHealth()), healthController);
            GameObject healthBarGameObject = Instantiate(healthBarPrefab, transform);
            HealthBar healthBar = healthBarGameObject.GetComponent<HealthBar>();
            _healthBars.Add(healthBar);
            healthBar.healthController = healthController;
        }
    }
}
