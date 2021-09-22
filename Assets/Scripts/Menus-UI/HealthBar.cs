using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Health_UI
{
    public class HealthBar : MonoBehaviour
    {
        public Health healthController;

        private RectTransform _rectTransform;
        private float _initialWidth;

        [SerializeField] private Text healthText;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialWidth = _rectTransform.rect.width;
            healthText = GetComponentInChildren<Text>();
        }

        private void Update()
        {
            if (!healthController)
            {
                Destroy(gameObject);
                return;
            }
            
            _rectTransform.sizeDelta = new Vector2(_initialWidth * healthController.GetPercentageHealth(), _rectTransform.rect.height);

            healthText.text = healthController.GetRoundedHealth() + " / " + healthController.GetInitialHealth();

        }
    }
}