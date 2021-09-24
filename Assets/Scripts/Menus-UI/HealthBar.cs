using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Player_UI
{
    /// <summary>
    /// Handles displaying the health component of the object assigned to it.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Description("The healthController in contorl of the healthbar. Things such as setting all heathbars inactive will be done through here")]
        [HideInInspector] public Health healthController;

        [Description("Controls the dimensions and size of the object")]
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
            
            // Sets width of image according to the percentage of health left.
            _rectTransform.sizeDelta = new Vector2(_initialWidth * healthController.GetPercentageHealth(), _rectTransform.rect.height);
            
            // Displays health information in this format: "current_health / max_health"
            healthText.text = healthController.GetRoundedHealth() + " / " + healthController.GetInitialHealth();

        }
    }
}