using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Health_UI
{
    public class HealthBarsLayout : MonoBehaviourPunCallbacks
    {
        public HealthBarsLayout Singleton;
        
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private int maxBars;
        private List<HealthBar> _healthBars;
        private SortedList<int, Health> _healthControllers;

        private void Awake()
        {

            if (Singleton)
            {
                Destroy(this);
                return;
            }

            Singleton = this;
            _healthControllers = new SortedList<int, Health>();
            _healthBars = new List<HealthBar>();
        }

        private void Start()
        {
            for (int i = 0; i < maxBars; i++)
            {
                GameObject healthBar = Instantiate(healthBarPrefab, transform);
                _healthBars[i] = healthBar.GetComponent<HealthBar>();
                healthBar.SetActive(false);
            }
        }

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
